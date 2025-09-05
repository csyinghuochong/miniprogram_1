using System;
using Unity.Mathematics;

namespace ET.Client
{
    [FriendOf(typeof(PlayerInfoComponent))]
    public static partial class EnterMapHelper
    {
        public static async ETTask<int> RequestTransfer(Scene root, int newsceneType, int sceneId, string paraminfo = "0")
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
            // request.Difficulty = difficulty;
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
    }
}