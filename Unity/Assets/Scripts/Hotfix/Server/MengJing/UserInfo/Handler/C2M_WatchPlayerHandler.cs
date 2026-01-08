namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponent))]
    public class C2M_WatchPlayerHandler : MessageLocationHandler<Unit, C2M_WatchPlayer, M2C_WatchPlayer>
    {
        protected override async ETTask Run(Unit unit, C2M_WatchPlayer request, M2C_WatchPlayer response)
        {
            UserInfoComponent userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponent>(unit.Root(), request.UnitId);
            if (userInfoComponent == null)
            {
                response.Error = ErrorCode.ERR_ComponentIsNull;
                return;
            }

            HeroComponent heroComponent = await UnitCacheHelper.GetComponentCache<HeroComponent>(unit.Root(), request.UnitId);
            if (heroComponent == null)
            {
                response.Error = ErrorCode.ERR_ComponentIsNull;
                return;
            }

            NumericComponent numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponent>(unit.Root(), request.UnitId);
            if (numericComponent == null)
            {
                response.Error = ErrorCode.ERR_ComponentIsNull;
                return;
            }

            WatchPlayerInfo watchPlayerInfo = WatchPlayerInfo.Create();
            watchPlayerInfo.UnitId = request.UnitId;
            watchPlayerInfo.PlayerName = userInfoComponent.GetPlayerName();
            watchPlayerInfo.AllianceName = ""; //先为空
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