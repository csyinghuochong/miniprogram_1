namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ActiveArchiveHeroHandler : MessageLocationHandler<Unit, C2M_ActiveArchiveHero, M2C_ActiveArchiveHero>
    {
        protected override async ETTask Run(Unit unit, C2M_ActiveArchiveHero request, M2C_ActiveArchiveHero response)
        {
            ArchiveComponent archiveComponent = unit.GetComponent<ArchiveComponent>();
            HeroComponent heroComponent = unit.GetComponent<HeroComponent>();

            Hero hero = heroComponent.GetHeroByConfigId(request.HeroConfigId);
            if (hero == null)
            {
                response.Error = ErrorCode.ERR_NotExistHero;
                return;
            }

            archiveComponent.ActiveArchiveHero(hero);

            await ETTask.CompletedTask;
        }
    }
}