namespace ET
{
    [ChildOf]
    public class FriendData : Entity, IAwake, IDestroy
    {
        public long UnitId { get; set; }
        public int OnLine { get; set; }
        public string PlayerName { get; set; }
        public int Lv { get; set; }
        public long CombatPower { get; set; }
    }
}