namespace ET.Server
{
    [MessageHandler(SceneType.Rank)]
    public class M2Rank_UpdatePlayerRankDataHandler : MessageLocationHandler<RankUnit, M2Rank_UpdatePlayerRankData, Rank2M_UpdatePlayerRankData>
    {
        protected override async ETTask Run(RankUnit rankUnit, M2Rank_UpdatePlayerRankData request, Rank2M_UpdatePlayerRankData response)
        {
            RankCenterComponent rankCenterComponent = rankUnit.Root().GetComponent<RankCenterComponent>();
            rankCenterComponent.UpdatePlayerCombatPowerRank(request.UnitId, request.PlayerName, request.CombatPower);

            await ETTask.CompletedTask;
        }
    }
}