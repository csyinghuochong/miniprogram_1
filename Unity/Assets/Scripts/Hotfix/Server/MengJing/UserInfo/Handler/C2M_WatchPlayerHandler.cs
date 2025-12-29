namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class C2M_WatchPlayerHandler : MessageLocationHandler<Unit, C2M_WatchPlayer, M2C_WatchPlayer>
    {
        protected override async ETTask Run(Unit unit, C2M_WatchPlayer request, M2C_WatchPlayer response)
        {
            UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(unit.Root(), request.UnitId);
            if (userInfoComponent == null)
            {
                response.Error = ErrorCode.ERR_ComponentIsNull;
                return;
            }

            HeroComponentS heroComponent = await UnitCacheHelper.GetComponentCache<HeroComponentS>(unit.Root(), request.UnitId);
            if (heroComponent == null)
            {
                response.Error = ErrorCode.ERR_ComponentIsNull;
                return;
            }

            NumericComponentS numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponentS>(unit.Root(), request.UnitId);
            if (numericComponent == null)
            {
                response.Error = ErrorCode.ERR_ComponentIsNull;
                return;
            }

            WatchPlayerInfo watchPlayerInfo = WatchPlayerInfo.Create();
            watchPlayerInfo.UnitId = request.UnitId;
            watchPlayerInfo.PlayerName = userInfoComponent.GetPlayerName();
            watchPlayerInfo.CombatPower = numericComponent.GetAsLong(NumericType.CombatPower);
            watchPlayerInfo.HeroFormation.AddRange(heroComponent.Formation);
            foreach (long id in heroComponent.Formation)
            {
                if (heroComponent.ChildrenDB == null)
                {
                    continue;
                }

                foreach (Entity entity in heroComponent.ChildrenDB)
                {
                    Hero hero = entity as Hero;
                    if (hero.Id == id)
                    {
                        watchPlayerInfo.HeroInfoList.Add(hero.ToMessage());
                    }
                }
            }

            response.WatchPlayerInfo = watchPlayerInfo;

            await ETTask.CompletedTask;
        }
    }
}