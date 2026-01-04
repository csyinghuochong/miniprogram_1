namespace ET
{
    public enum RankType
    {
        PlayerRank = 1, //玩家战力排行榜
    }

    [ChildOf]
    public class RankData : Entity, IAwake, IDestroy
    {
        public int RankType { get; set; }
        public long UnitId { get; set; }
        public string PlayerName { get; set; }
        public long CombatPower { get; set; }
    }
}