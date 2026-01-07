namespace ET.Server
{
    [FriendOf(typeof(ArchiveComponentS))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GetAllArchiveHeroHandler : MessageLocationHandler<Unit, C2M_GetAllArchiveHero, M2C_GetAllArchiveHero>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllArchiveHero request, M2C_GetAllArchiveHero response)
        {
            ArchiveComponentS archiveComponent = unit.GetComponent<ArchiveComponentS>();

            response.ReceivedArchiveRewardIds.AddRange(archiveComponent.ReceivedArchiveRewardIds);
            foreach (ArchiveHero archiveHero in archiveComponent.ArchiveHeroList)
            {
                response.ArchiveHeroInfoList.Add(archiveHero.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}