namespace ET
{
    public struct RewardItem
    {
        public int ItemId;
        public int ItemNum;
    }

    public enum ItemOpType
    {
        Add,
        Remove,
        Update,
    }

    public enum InventoryContainerType
    {
        None = 0, //无类型
        Bag = 1, //背包
    }

    [ChildOf]
    public class Item : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int ContainerType { get; set; }
        public int Num { get; set; }
    }
}