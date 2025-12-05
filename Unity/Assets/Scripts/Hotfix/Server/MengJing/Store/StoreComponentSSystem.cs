namespace ET.Server
{
    [EntitySystemOf(typeof(StoreComponentS))]
    [FriendOf(typeof(StoreComponentS))]
    public static partial class StoreComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this StoreComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this StoreComponentS self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this StoreComponentS self)
        {
        }

        public static void Check(this StoreComponentS self)
        {
            if (self.StoreItemList.Count == 0)
            {
                self.RefreshStore();
            }
            else
            {
                if (TimeHelper.ServerNow() > self.RefreshTime)
                {
                    self.RefreshStore();
                }
            }
        }

        public static void RefreshStore(this StoreComponentS self)
        {
            self.RefreshTime = TimeHelper.ServerNow() + ConfigData.StoreRefreshTime;
            self.StoreItemList.Clear();

            foreach (StoreItemConfig storeItemConfig in StoreItemConfigCategory.Instance.DataList)
            {
                self.StoreItemList.Add(storeItemConfig.Id, storeItemConfig.LimitNumber);
            }
        }
    }
}