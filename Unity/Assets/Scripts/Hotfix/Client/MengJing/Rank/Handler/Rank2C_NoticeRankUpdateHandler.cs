namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Rank2C_NoticeRankUpdateHandler : MessageHandler<Scene, Rank2C_NoticeRankUpdate>
    {
        protected override async ETTask Run(Scene root, Rank2C_NoticeRankUpdate message)
        {
            RankComponent rankComponent = root.GetComponent<RankComponent>();
            rankComponent?.RankUpdate(message.RankDataInfoList);

            await ETTask.CompletedTask;
        }
    }
}