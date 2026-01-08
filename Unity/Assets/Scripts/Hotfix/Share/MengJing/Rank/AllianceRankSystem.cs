namespace ET
{
    [EntitySystemOf(typeof(AllianceRank))]
    public static partial class AllianceRankSystem
    {
        [EntitySystem]
        private static void Awake(this AllianceRank self)
        {
        }

        [EntitySystem]
        private static void Destroy(this AllianceRank self)
        {
        }

        public static AllianceRankInfo ToMessage(this AllianceRank self)
        {
            AllianceRankInfo info = AllianceRankInfo.Create();
            info.Sort = self.Sort;
            info.AllianceId = self.AllianceId;
            info.AllianceName = self.AllianceName;
            info.Active = self.Active;
            return info;
        }

        public static void FromMessage(this AllianceRank self, AllianceRankInfo info)
        {
            self.Sort = info.Sort;
            self.AllianceId = info.AllianceId;
            self.AllianceName = info.AllianceName;
            self.Active = info.Active;
        }
    }
}