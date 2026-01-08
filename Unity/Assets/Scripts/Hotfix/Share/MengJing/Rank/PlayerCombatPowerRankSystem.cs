namespace ET
{
    [EntitySystemOf(typeof(PlayerCombatPowerRank))]
    [FriendOf(typeof(PlayerCombatPowerRank))]
    public static partial class PlayerCombatPowerRankSystem
    {
        [EntitySystem]
        private static void Awake(this PlayerCombatPowerRank self)
        {
        }

        [EntitySystem]
        private static void Destroy(this PlayerCombatPowerRank self)
        {
        }

        public static PlayerCombatPowerRankInfo ToMessage(this PlayerCombatPowerRank self)
        {
            PlayerCombatPowerRankInfo info = PlayerCombatPowerRankInfo.Create();
            info.Sort = self.Sort;
            info.UnitId = self.UnitId;
            info.PlayerName = self.PlayerName;
            info.CombatPower = self.CombatPower;
            return info;
        }

        public static void FromMessage(this PlayerCombatPowerRank self, PlayerCombatPowerRankInfo info)
        {
            self.Sort = info.Sort;
            self.UnitId = info.UnitId;
            self.PlayerName = info.PlayerName;
            self.CombatPower = info.CombatPower;
        }
    }
}