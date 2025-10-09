namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class C2M_SetHeroCurrentFormationIndexHandler : MessageLocationHandler<Unit, C2M_SetHeroCurrentFormationIndex, M2C_SetHeroCurrentFormationIndex>
    {
        protected override async ETTask Run(Unit unit, C2M_SetHeroCurrentFormationIndex request, M2C_SetHeroCurrentFormationIndex response)
        {
            if (request.CurrentFormationIndex < 1 || request.CurrentFormationIndex > 2)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            HeroComponentS heroComponentS = unit.GetComponent<HeroComponentS>();
            heroComponentS.CurrentFormationIndex = request.CurrentFormationIndex;

            await ETTask.CompletedTask;
        }
    }
}