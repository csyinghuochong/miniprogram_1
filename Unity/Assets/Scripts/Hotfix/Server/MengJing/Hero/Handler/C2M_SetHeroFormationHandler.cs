namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class C2M_SetHeroFormationHandler : MessageLocationHandler<Unit, C2M_SetHeroFormation, M2C_SetHeroFormation>
    {
        protected override async ETTask Run(Unit unit, C2M_SetHeroFormation request, M2C_SetHeroFormation response)
        {
            HeroComponentS heroComponentS = unit.GetComponent<HeroComponentS>();

            int error = heroComponentS.SetFormation(request.OpType, request.HeroId, request.SlotIndex);

            if (error != ErrorCode.ERR_Success)
            {
                response.Error = error;
                return;
            }

            response.Formation = heroComponentS.Formation;

            await ETTask.CompletedTask;
        }
    }
}