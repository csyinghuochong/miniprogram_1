namespace ET
{
    [ChildOf]
    public class BattlePass : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public bool RewardReceived_1 { get; set; }
        public bool RewardReceived_2 { get; set; }
        public bool RewardReceived_3 { get; set; }
    }
}