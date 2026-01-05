namespace ET.Server
{
    [MessageHandler(SceneType.Rank)]
    public class G2Rank_LoginRankServerHandler : MessageHandler<Scene, G2Rank_LoginRankServer, Rank2G_LoginRankServer>
    {
        protected override async ETTask Run(Scene scene, G2Rank_LoginRankServer request, Rank2G_LoginRankServer response)
        {
            RankCenterComponent rankCenterComponent = scene.GetComponent<RankCenterComponent>();
            rankCenterComponent.UpdatePlayerRankData(request.UnitId, request.PlayerName, request.CombatPower);

            RankUnitComponent rankUnitComponent = scene.GetComponent<RankUnitComponent>();
            rankUnitComponent.Children.TryGetValue(request.UnitId, out Entity rankUnitEntity);

            RankUnit rankUnit = rankUnitEntity as RankUnit;

            if (rankUnit != null)
            {
                return;
            }

            rankUnit = rankUnitComponent.AddChildWithId<RankUnit>(request.UnitId);
            rankUnit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);

            await rankUnit.AddLocation(LocationType.Rank);
        }
    }
}