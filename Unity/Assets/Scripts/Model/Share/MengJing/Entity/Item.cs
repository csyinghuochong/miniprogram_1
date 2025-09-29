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

    public enum ItemType
    {
        Consume = 1, //消耗
        Material = 2, //材料
        Equipment = 3, //装备
        HeroShard = 4, //英雄碎片
    }

    [ChildOf]
    public class Item : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public int ConfigId { get; set; }
        public int ContainerType { get; set; }
        public int Num { get; set; }
    }
}