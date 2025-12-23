namespace ET
{
    [ChildOf]
    public class ChatEntry : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public long UnitId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Channel { get; set; }
    }
}