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
                MapType oldMapType = unit.Scene().GetComponent<MapComponent>().MapType;

                // 从主城传送到其他地图，保存在主城的坐标
                if (oldMapType == MapType.MainCity && request.MapType != (int)MapType.MainCity)
                {
                    UnitHelper.SaveUnitMainCityPos(unit);
                }

                switch (request.MapType)
                {
                    case (int)MapType.MainCity:
                    {
                        if (oldMapType == MapType.MainCity)
                        {
                            unit.Position = new float3(0, 0, 0);
                            unit.Stop(0);
                            return ErrorCode.ERR_Success;
                        }

                        ActorId mapInstanceId = UnitCacheHelper.MainCityServerId(unit.Zone());
                        BeforeTransfer(unit, oldMapType);
                        await Transfer(unit, mapInstanceId, MapType.MainCity, 101);

                        break;
                    }
                    case (int)MapType.LocalLevel:
                    {
                        long levelId = IdGenerater.Instance.GenerateId();
                        long levelInstanceId = IdGenerater.Instance.GenerateInstanceId();
                        Scene levelScene = GateMapFactory.Create(unit.Root(), levelId, levelInstanceId, $"LocalLevel{levelId}");

                        MapComponent mapComponent = levelScene.GetComponent<MapComponent>();
                        mapComponent.SetMapInfo(MapType.LocalLevel, request.SceneId);

                        // levelScene.AddComponent<CrowdComponent, byte[]>(NavmeshComponent.Instance.Get(CommonHelp.GetMapObjName(MapType.LocalLevel)));

                        levelScene.AddComponent<LocalLevelComponent>();

                        BeforeTransfer(unit, oldMapType);
                        await Transfer(unit, levelScene.GetActorId(), MapType.LocalLevel, request.SceneId);

                        break;
                    }
                    default:
                        break;
                }

                // 单人关卡销毁
                if (oldMapType == MapType.LocalLevel)
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

            await Transfer(unit, sceneInstanceId, MapType.MainCity, 101);
        }

        public static void OnPlayerDisconnect(Scene scene, long userId)
        {
            MapType sceneType = scene.GetComponent<MapComponent>().MapType;

            // 单人关卡销毁
            if (sceneType == MapType.LocalLevel)
            {
                scene.Dispose();
                return;
            }
        }

        public static void BeforeTransfer(Unit unit, MapType sceneType)
        {
            // 删除unit,让其它进程发送过来的消息找不到actor，重发
            // Game.EventSystem.Remove(unitId);
            // 删除Mailbox,让发给Unit的ActorLocation消息重发

            unit.RemoveComponent<MailBoxComponent>();
        }

        public static void AfterTransfer(Unit unit, MapType mapType)
        {
        }

        private static async ETTask Transfer(Unit unit, ActorId sceneInstanceId, MapType mapType, int sceneId)
        {
            Scene root = unit.Root();
            // location加锁
            long unitId = unit.Id;

            M2M_UnitTransferRequest request = M2M_UnitTransferRequest.Create();
            request.OldActorId = unit.GetActorId();
            request.Unit = unit.ToBson();
            request.MapType = (int)mapType;
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