namespace ET
{
    [ChildOf]
    public class AllianceRank : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int Sort { get; set; }
        public long AllianceId { get; set; }
        public string AllianceName { get; set; }
        public long Active { get; set; }
    }
}