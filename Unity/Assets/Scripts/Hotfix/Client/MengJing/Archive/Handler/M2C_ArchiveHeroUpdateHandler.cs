namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_ArchiveHeroUpdateHandler : MessageHandler<Scene, M2C_ArchiveHeroUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_ArchiveHeroUpdate message)
        {
            ArchiveComponentC archiveComponent = root.GetComponent<ArchiveComponentC>();
            archiveComponent?.AddOrUpdateArchiveHero(message.ArchiveHeroInfo);

            await ETTask.CompletedTask;
        }
    }
}