namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class C2M_SetHeroCurrentFormationIndexHandler : MessageLocationHandler<Unit, C2M_SetHeroCurrentFormationIndex,
        M2C_SetHeroCurrentFormationIndex>
    {
        protected override async ETTask Run(Unit unit, C2M_SetHeroCurrentFormationIndex request, M2C_SetHeroCurrentFormationIndex response)
        {
            HeroComponentS heroComponentS = unit.GetComponent<HeroComponentS>();

            if (request.CurrentFormationIndex < 1 || request.CurrentFormationIndex > heroComponentS.MaxFormationIndex)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            heroComponentS.CurrentFormationIndex = request.CurrentFormationIndex;

            await ETTask.CompletedTask;
        }
    }
}