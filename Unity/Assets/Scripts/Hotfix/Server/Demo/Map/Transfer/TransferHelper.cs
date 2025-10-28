using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    public static partial class TransferHelper
    {
        public static async ETTask<int> TransferUnit(Unit unit, C2M_TransferMap request)
        {
            using (await unit.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Transfer, unit.Id))
            {
                if (unit.IsDisposed)
                {
                    return ErrorCode.ERR_RequestRepeatedly;
                }

                Scene oldScene = unit.Scene();
                int oldMapType = unit.Scene().GetComponent<MapComponent>().MapType;

                // 从主城传送到其他地图，保存在主城的坐标
                if (oldMapType == MapTypeEnum.MainCityScene && request.SceneType != MapTypeEnum.MainCityScene)
                {
                    NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();
                    numericComponent.ApplyValue(NumericType.MainCity_X, unit.Position.x);
                    numericComponent.ApplyValue(NumericType.MainCity_Y, unit.Position.y);
                    numericComponent.ApplyValue(NumericType.MainCity_Z, unit.Position.z);
                }

                switch (request.SceneType)
                {
                    case MapTypeEnum.MainCityScene:
                    {
                        if (oldMapType == MapTypeEnum.MainCityScene)
                        {
                            unit.Position = new float3(0, 0, 0);
                            unit.Stop(0);
                            return ErrorCode.ERR_Success;
                        }

                        ActorId mapInstanceId = UnitCacheHelper.MainCityServerId(unit.Zone());
                        BeforeTransfer(unit, oldMapType);
                        await Transfer(unit, mapInstanceId, MapTypeEnum.MainCityScene, 101);

                        break;
                    }
                    case MapTypeEnum.LocalLevel:
                    {
                        long levelId = IdGenerater.Instance.GenerateId();
                        long levelInstanceId = IdGenerater.Instance.GenerateInstanceId();
                        Scene levelScene = GateMapFactory.Create(unit.Root(), levelId, levelInstanceId, $"LocalLevel{levelId}");

                        MapComponent mapComponent = levelScene.GetComponent<MapComponent>();
                        mapComponent.SetMapInfo(MapTypeEnum.LocalLevel, request.SceneId);

                        BeforeTransfer(unit, oldMapType);
                        await Transfer(unit, levelScene.GetActorId(), MapTypeEnum.LocalLevel, request.SceneId);

                        break;
                    }
                    default:
                        break;
                }

                // 单人关卡销毁
                if (oldMapType == MapTypeEnum.LocalLevel)
                {
                    oldScene.Dispose();
                    return ErrorCode.ERR_Success;
                }
            }

            return ErrorCode.ERR_Success;
        }

        public static async ETTask TransferAtFrameFinish(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            await unit.Fiber().WaitFrameFinish();

            await Transfer(unit, sceneInstanceId, MapTypeEnum.MainCityScene, 101);
        }

        public static void OnPlayerDisconnect(Scene scene, long userId)
        {
            int sceneTypeEnum = scene.GetComponent<MapComponent>().MapType;

            // 单人关卡销毁
            if (sceneTypeEnum == MapTypeEnum.LocalLevel)
            {
                scene.Dispose();
                return;
            }
        }

        private static void BeforeTransfer(Unit unit, int sceneType)
        {
            //删除unit,让其它进程发送过来的消息找不到actor，重发
            //Game.EventSystem.Remove(unitId);
            // 删除Mailbox,让发给Unit的ActorLocation消息重发

            unit.RemoveComponent<MailBoxComponent>();
        }

        public static void AfterTransfer(Unit unit, int sceneType)
        {
        }

        private static async ETTask Transfer(Unit unit, ActorId sceneInstanceId, int sceneType, int sceneId)
        {
            Scene root = unit.Root();
            // location加锁
            long unitId = unit.Id;

            M2M_UnitTransferRequest request = M2M_UnitTransferRequest.Create();
            request.OldActorId = unit.GetActorId();
            request.Unit = unit.ToBson();
            request.SceneType = sceneType;
            request.SceneId = sceneId;

            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(entity.ToBson());
                }
            }

            unit.GetParent<UnitComponent>().Remove(unit.Id);
            await root.GetComponent<TimerComponent>().WaitFrameAsync();
            await root.GetComponent<LocationProxyComponent>().Lock(LocationType.Unit, unitId, request.OldActorId);
            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
        }
    }
}