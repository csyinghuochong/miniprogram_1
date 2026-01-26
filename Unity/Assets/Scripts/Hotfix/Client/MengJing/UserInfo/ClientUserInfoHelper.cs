namespace ET.Client
{
    public static class ClientUserInfoHelper
    {
        public static async ETTask<int> RequestGetUserInfo(Scene root)
        {
            C2M_GetUserInfo request = C2M_GetUserInfo.Create();

            M2C_GetUserInfo response = (M2C_GetUserInfo)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            UserInfoComponentC userInfoComponent = root.GetComponent<UserInfoComponentC>();
            userInfoComponent.PlayerName = response.PlayerName;
            userInfoComponent.Gold = response.Gold;
            userInfoComponent.Diamond = response.Diamond;
            userInfoComponent.Exp = response.Exp;
            userInfoComponent.Lv = response.Lv;
            userInfoComponent.RechargeNumber = response.RechargeNumber;

            return response.Error;
        }

        public static async ETTask<M2C_WatchPlayer> WatchPlayer(Scene root, long unitId)
        {
            C2M_WatchPlayer request = C2M_WatchPlayer.Create();
            request.UnitId = unitId;

            M2C_WatchPlayer response = (M2C_WatchPlayer)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response;
        }

        public static async ETTask<int> Recharge(Scene root, int configId)
        {
            C2M_Recharge request = C2M_Recharge.Create();
            request.ConfigId = configId;

            M2C_Recharge response = (M2C_Recharge)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }
    }
}