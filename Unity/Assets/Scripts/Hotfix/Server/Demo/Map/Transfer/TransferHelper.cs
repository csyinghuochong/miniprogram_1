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

                int oldScene = unit.Scene().GetComponent<MapComponent>().MapType;

                switch (request.SceneType)
                {
                    case MapTypeEnum.MainCityScene:
                        await MainCityTransfer(unit);
                        break;
                    default:
                        break;
                }
            }

            return ErrorCode.ERR_Success;
        }

        public static async ETTask TransferAtFrameFinish(Unit unit, ActorId sceneInstanceId, string sceneName)
        {
            await unit.Fiber().WaitFrameFinish();

            await Transfer(unit, sceneInstanceId, MapTypeEnum.MainCityScene, 101, 1, "0");
        }

        public static async ETTask MainCityTransfer(Unit unit)
        {
            MapComponent mapComponent = unit.Scene().GetComponent<MapComponent>();
            if (mapComponent.MapType == MapTypeEnum.MainCityScene)
            {
                OnMainToMain(unit);
                return;
            }
            
            //传送回主场景
            ActorId mapInstanceId = UnitCacheHelper.MainCityServerId(unit.Zone());
            long userId = unit.Id;
            Scene scene = unit.Scene();
            BeforeTransfer(unit, mapComponent.MapType);
            await Transfer(unit, mapInstanceId, (int)MapTypeEnum.MainCityScene, GlobalValueConfigCategory.Instance.MainCityID, 0, "0");
        }

        private static void OnMainToMain(Unit unit)
        {
            SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(GlobalValueConfigCategory.Instance.MainCityID);
            unit.Position = new float3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
            unit.Stop(-2);
        }

        public static void OnPlayerDisconnect(Scene scene, long userId)
        {
        }

        public static void BeforeTransfer(Unit unit, int sceneType)
        {
            //删除unit,让其它进程发送过来的消息找不到actor，重发
            //Game.EventSystem.Remove(unitId);
            // 删除Mailbox,让发给Unit的ActorLocation消息重发

            unit.RemoveComponent<MailBoxComponent>();
        }

        public static void AfterTransfer(Unit unit, int sceneType)
        {
        }

        public static async ETTask Transfer(Unit unit, ActorId sceneInstanceId, int sceneType, int sceneId, int fubenDifficulty, string paramInfo)
        {
            Scene root = unit.Root();
            // location加锁
            long unitId = unit.Id;

            M2M_UnitTransferRequest request = M2M_UnitTransferRequest.Create();
            request.OldActorId = unit.GetActorId();
            request.Unit = unit.ToBson();
            request.SceneType = sceneType;
            request.SceneId = sceneId;
            request.FubenDifficulty = fubenDifficulty;
            request.ParamInfo = paramInfo;

            foreach (Entity entity in unit.Components.Values)
            {
                if (entity is ITransfer)
                {
                    request.Entitys.Add(entity.ToBson());
                }
                else
                {
                }
            }

            //unit.Dispose();
            unit.GetParent<UnitComponent>().Remove(unit.Id);
            await root.GetComponent<TimerComponent>().WaitFrameAsync();
            await root.GetComponent<LocationProxyComponent>().Lock(LocationType.Unit, unitId, request.OldActorId);
            await root.GetComponent<MessageSender>().Call(sceneInstanceId, request);
        }
    }
}