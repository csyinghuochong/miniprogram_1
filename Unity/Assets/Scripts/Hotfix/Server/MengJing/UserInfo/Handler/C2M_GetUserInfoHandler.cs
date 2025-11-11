namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(UserInfoComponentS))]
    public class C2M_GetUserInfoHandler : MessageLocationHandler<Unit, C2M_GetUserInfo, M2C_GetUserInfo>
    {
        protected override async ETTask Run(Unit unit, C2M_GetUserInfo request, M2C_GetUserInfo response)
        {
            UserInfoComponentS userInfoComponent = unit.GetComponent<UserInfoComponentS>();
            response.PlayerName = userInfoComponent.PlayerName;
            response.Gold = userInfoComponent.Gold;
            response.Diamond = userInfoComponent.Diamond;
            response.Exp = userInfoComponent.Exp;
            response.Lv = userInfoComponent.Lv;

            await ETTask.CompletedTask;
        }
    }
}