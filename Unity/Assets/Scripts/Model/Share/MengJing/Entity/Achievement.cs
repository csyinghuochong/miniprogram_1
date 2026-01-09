namespace ET
{
    [ChildOf]
    public class Achievement : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int Progress { get; set; }
    }
}