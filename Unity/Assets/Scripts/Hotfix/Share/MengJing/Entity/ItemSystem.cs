namespace ET
{
    [EntitySystemOf(typeof(Item))]
    [FriendOf(typeof(Item))]
    public static partial class ItemSystem
    {
        [EntitySystem]
        private static void Awake(this Item self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Item self)
        {
        }

        public static ItemInfo ToMessage(this Item self)
        {
            ItemInfo itemInfo = ItemInfo.Create();
            itemInfo.Id = self.Id;
            itemInfo.ConfigId = self.ConfigId;
            itemInfo.ContainerType = self.ContainerType;
            itemInfo.Num = self.Num;

            return itemInfo;
        }

        public static void FromMessage(this Item self, ItemInfo itemInfo)
        {
            self.ConfigId = itemInfo.ConfigId;
            self.ContainerType = itemInfo.ContainerType;
            self.Num = itemInfo.Num;
        }
    }
}