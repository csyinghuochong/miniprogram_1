using System;
using Unity.Mathematics;

namespace ET.Client
{
    [FriendOf(typeof(PlayerInfoComponent))]
    public static partial class EnterMapHelper
    {
        public static async ETTask<int> RequestTransfer(Scene root, int newsceneType, int sceneId, int difficulty = FubenDifficulty.None,
        string paraminfo = "0")
        {
            MapComponent mapComponent = root.GetComponent<MapComponent>();
            if (TimeHelper.ServerNow() - mapComponent.LastQuitTime < 2000)
            {
                return ErrorCode.ERR_OperationOften;
            }

            mapComponent.LastQuitTime = TimeHelper.ServerNow();

            C2M_TransferMap request = C2M_TransferMap.Create();
            request.SceneType = newsceneType;
            request.SceneId = sceneId;
            request.Difficulty = difficulty;
            request.paramInfo = paraminfo;

            M2C_TransferMap response = (M2C_TransferMap)await root.GetComponent<ClientSenderCompnent>().Call(request);

            return ErrorCode.ERR_Success;
        }

        public static async ETTask Match(Fiber fiber)
        {
            try
            {
                G2C_Match g2CEnterMap = await fiber.Root.GetComponent<ClientSenderCompnent>().Call(C2G_Match.Create()) as G2C_Match;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void RequestQuitFuben(Scene zoneScene)
        {
            RequestTransfer(zoneScene, (int)MapTypeEnum.MainCityScene, GlobalValueConfigCategory.Instance.MainCityID).Coroutine();
        }

        public static async ETTask<int> RequestFlyToPosition(Scene root, int unitType, int configid)
        {
            MoveHelper.Stop(root);
            C2M_FlyToPosition c2mFlyToPosition = C2M_FlyToPosition.Create();
            c2mFlyToPosition.UnitType = unitType;
            c2mFlyToPosition.ConfigId = configid;
            M2C_FlyToPosition response = await root.GetComponent<ClientSenderCompnent>().Call(c2mFlyToPosition) as M2C_FlyToPosition;
            return response.Error;
        }

        public static async ETTask SendReviveRequest(Scene root, bool revive)
        {
            Actor_SendReviveRequest request = Actor_SendReviveRequest.Create();
            request.Revive = revive;

            Actor_SendReviveResponse response = await root.GetComponent<ClientSenderCompnent>().Call(request) as Actor_SendReviveResponse;
        }
    }
}