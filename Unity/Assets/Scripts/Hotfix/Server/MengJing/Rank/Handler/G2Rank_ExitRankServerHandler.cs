namespace ET.Server
{
    [MessageHandler(SceneType.Rank)]
    public class G2Rank_ExitRankServerHandler : MessageLocationHandler<RankUnit, G2Rank_ExitRankServer, Rank2G_ExitRankServer>
    {
        protected override async ETTask Run(RankUnit rankUnit, G2Rank_ExitRankServer request, Rank2G_ExitRankServer response)
        {
            RankUnitExit(rankUnit).Coroutine();

            await ETTask.CompletedTask;
        }

        private async ETTask RankUnitExit(RankUnit rankUnit)
        {
            await rankUnit.Fiber().WaitFrameFinish();
            await rankUnit.RemoveLocation(LocationType.Rank);
            rankUnit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Remove(rankUnit.Id);
            rankUnit?.Dispose();
        }
    }
}