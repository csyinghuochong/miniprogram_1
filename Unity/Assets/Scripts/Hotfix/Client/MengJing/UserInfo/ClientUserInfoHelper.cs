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

            return response.Error;
        }
    }
}