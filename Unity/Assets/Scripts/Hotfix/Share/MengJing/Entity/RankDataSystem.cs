namespace ET
{
    [EntitySystemOf(typeof(RankData))]
    [FriendOf(typeof(RankData))]
    public static partial class RankDataSystem
    {
        [EntitySystem]
        private static void Awake(this RankData self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RankData self)
        {
        }

        public static RankDataInfo ToMessage(this RankData self)
        {
            RankDataInfo rankDataInfo = RankDataInfo.Create();
            rankDataInfo.RankType = self.RankType;
            rankDataInfo.Rank = self.Rank;
            rankDataInfo.UnitId = self.UnitId;
            rankDataInfo.PlayerName = self.PlayerName;
            rankDataInfo.CombatPower = self.CombatPower;
            return rankDataInfo;
        }

        public static void FromMessage(this RankData self, RankDataInfo rankDataInfo)
        {
            self.RankType = rankDataInfo.RankType;
            self.Rank = rankDataInfo.Rank;
            self.UnitId = rankDataInfo.UnitId;
            self.PlayerName = rankDataInfo.PlayerName;
            self.CombatPower = rankDataInfo.CombatPower;
        }
    }
}