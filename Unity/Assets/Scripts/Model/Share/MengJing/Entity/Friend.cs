namespace ET
{
    [ChildOf]
    public class Friend : Entity, IAwake, IDestroy
    {
        public long UnitId { get; set; }
        public int OnLine { get; set; }
        public string PlayerName { get; set; }
        public int Lv { get; set; }
    }
}