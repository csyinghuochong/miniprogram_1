namespace ET
{
    [ChildOf]
    public class Chat : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public long UnitId { get; set; }
        public long SendTime { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
    }
}