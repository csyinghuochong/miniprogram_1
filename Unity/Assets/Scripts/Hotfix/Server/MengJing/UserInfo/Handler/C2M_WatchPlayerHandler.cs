namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_WatchPlayerHandler : MessageLocationHandler<Unit, C2M_WatchPlayer, M2C_WatchPlayer>
    {
        protected override async ETTask Run(Unit unit, C2M_WatchPlayer request, M2C_WatchPlayer response)
        {
            UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(unit.Root(), request.UnitId);
            HeroComponentS heroComponent = await UnitCacheHelper.GetComponentCache<HeroComponentS>(unit.Root(), request.UnitId);

            if (userInfoComponent == null)
            {
                response.Error = ErrorCode.ERR_ComponentIsNull;
                return;
            }

            response.PlayerName = userInfoComponent.GetPlayerName();

            await ETTask.CompletedTask;
        }
    }
}