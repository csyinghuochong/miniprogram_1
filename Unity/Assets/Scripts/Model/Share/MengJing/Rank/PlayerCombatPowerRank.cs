namespace ET
{
    [ChildOf]
    public class PlayerCombatPowerRank : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int Sort { get; set; }
        public long UnitId { get; set; }
        public string PlayerName { get; set; }
        public long CombatPower { get; set; }
    }
}