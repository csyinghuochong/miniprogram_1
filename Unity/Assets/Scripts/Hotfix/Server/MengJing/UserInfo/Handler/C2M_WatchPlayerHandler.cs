namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class C2M_WatchPlayerHandler : MessageLocationHandler<Unit, C2M_WatchPlayer, M2C_WatchPlayer>
    {
        protected override async ETTask Run(Unit unit, C2M_WatchPlayer request, M2C_WatchPlayer response)
        {
            UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(unit.Root(), request.UnitId);
            HeroComponentS heroComponent = await UnitCacheHelper.GetComponentCache<HeroComponentS>(unit.Root(), request.UnitId);
            NumericComponentS numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponentS>(unit.Root(), request.UnitId);

            WatchPlayerInfo watchPlayerInfo = WatchPlayerInfo.Create();
            watchPlayerInfo.UnitId = request.UnitId;
            watchPlayerInfo.PlayerName = userInfoComponent.GetPlayerName();
            watchPlayerInfo.CombatPower = numericComponent.GetAsLong(NumericType.CombatPower);
            watchPlayerInfo.HeroFormation.AddRange(heroComponent.Formation);
            foreach (long id in heroComponent.Formation)
            {
                Hero hero = heroComponent.GetHero(id);
                if (hero != null)
                {
                    watchPlayerInfo.HeroInfoList.Add(hero.ToMessage());
                }
            }

            response.WatchPlayerInfo = watchPlayerInfo;

            await ETTask.CompletedTask;
        }
    }
}