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

        public static void OnLogin(this StoreComponentS self)
        {
            if (self.StoreItemList.Count == 0)
            {
                self.RefreshStore();
            }
            else
            {
                if (TimeHelper.ServerNow() - self.LastRefreshTime > ConfigData.StoreRefreshTime)
                {
                    self.RefreshStore();
                }
            }
        }

        public static void RefreshStore(this StoreComponentS self)
        {
            self.LastRefreshTime = TimeHelper.ServerNow();
            self.StoreItemList.Clear();

            foreach (StoreItemConfig storeItemConfig in StoreItemConfigCategory.Instance.DataList)
            {
                self.StoreItemList.Add(storeItemConfig.Id, storeItemConfig.LimitNumber);
            }
        }
    }
}