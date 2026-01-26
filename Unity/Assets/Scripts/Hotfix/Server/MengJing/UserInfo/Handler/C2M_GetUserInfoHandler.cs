namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(UserInfoComponent))]
    public class C2M_GetUserInfoHandler : MessageLocationHandler<Unit, C2M_GetUserInfo, M2C_GetUserInfo>
    {
        protected override async ETTask Run(Unit unit, C2M_GetUserInfo request, M2C_GetUserInfo response)
        {
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            response.PlayerName = userInfoComponent.PlayerName;
            response.Gold = userInfoComponent.Gold;
            response.Diamond = userInfoComponent.Diamond;
            response.Exp = userInfoComponent.Exp;
            response.Lv = userInfoComponent.Lv;
            response.RechargeNumber = userInfoComponent.RechargeNumber;

            await ETTask.CompletedTask;
        }
    }
}