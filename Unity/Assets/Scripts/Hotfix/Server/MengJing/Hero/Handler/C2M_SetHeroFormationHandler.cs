namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class C2M_SetHeroFormationHandler : MessageLocationHandler<Unit, C2M_SetHeroFormation, M2C_SetHeroFormation>
    {
        protected override async ETTask Run(Unit unit, C2M_SetHeroFormation request, M2C_SetHeroFormation response)
        {
            HeroComponentS heroComponent = unit.GetComponent<HeroComponentS>();

            int error = heroComponent.SetFormation(request.OpType, request.HeroId, request.SlotIndex);

            if (error != ErrorCode.ERR_Success)
            {
                response.Error = error;
                return;
            }

            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();
            if (heroComponent.GetFirstHero() != null)
            {
                numericComponent.ApplyValue(NumericType.ShowHeroId, heroComponent.GetFirstHero().ConfigId);
            }
            
            response.Formation.AddRange(heroComponent.Formation);

            await ETTask.CompletedTask;
        }
    }
}