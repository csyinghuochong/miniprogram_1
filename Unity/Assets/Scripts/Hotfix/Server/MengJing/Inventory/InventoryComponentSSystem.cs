using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [EntitySystemOf(typeof(InventoryComponentS))]
    [FriendOf(typeof(InventoryComponentS))]
    public static partial class InventoryComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this InventoryComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this InventoryComponentS self)
        {
            self.Items.Clear();
            self.Items = null;
        }

        [EntitySystem]
        private static void Deserialize(this InventoryComponentS self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Item item)
                {
                    self.Items.Add(item.Id, item);
                }
            }
        }

        public static Item GetItem(this InventoryComponentS self, long itemId)
        {
            self.Items.TryGetValue(itemId, out EntityRef<Item> item);
            return item;
        }

        public static void AddItemData(this InventoryComponentS self, List<RewardItem> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
        {
            for (int i = rewardItems.Count - 1; i >= 0; i--)
            {
                int itemConfigId = rewardItems[i].ItemId;
                int leftNum = rewardItems[i].ItemNum;

                if (!ItemConfigCategory.Instance.DataMap.ContainsKey(itemConfigId))
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemConfigId);

                if (itemConfigId <= 10000000)
                {
                    // 加到玩家数据中
                    UserInfoComponentS userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponentS>();
                    switch (itemConfig.Id)
                    {
                        case ConfigData.Item_Gold:
                            userInfoComponent.ChangeRoleData(UserDataType.Gold, leftNum);
                            break;
                        case ConfigData.Item_Diamond:
                            userInfoComponent.ChangeRoleData(UserDataType.Diamond, leftNum);
                            break;
                        case ConfigData.Item_Exp:
                            userInfoComponent.ChangeRoleData(UserDataType.Exp, leftNum);
                            break;
                    }

                    continue;
                }

                foreach (Item item in self.Items.Values)
                {
                    if ((int)containerType == item.ContainerType && itemConfigId == item.ConfigId)
                    {
                        if (item.Num < itemConfig.ItemPileSum)
                        {
                            if (item.Num + leftNum > itemConfig.ItemPileSum)
                            {
                                item.Num = itemConfig.ItemPileSum;
                                leftNum = item.Num + leftNum - itemConfig.ItemPileSum;
                            }
                            else
                            {
                                item.Num = item.Num + leftNum;
                                leftNum = 0;
                            }

                            ItemNoticeHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Update);
                        }
                    }
                }

                while (leftNum > 0)
                {
                    Item newItem = self.AddChild<Item>();
                    newItem.ConfigId = itemConfigId;
                    newItem.ContainerType = (int)containerType;

                    if (leftNum > itemConfig.ItemPileSum)
                    {
                        newItem.Num = itemConfig.ItemPileSum;
                        leftNum -= itemConfig.ItemPileSum;
                    }
                    else
                    {
                        newItem.Num = leftNum;
                        leftNum = 0;
                    }

                    self.AddItem(newItem);
                }
            }
        }

        public static bool HaveItemData(this InventoryComponentS self, List<RewardItem> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
        {
            Dictionary<int, int> removeItems = new();
            foreach (RewardItem rewardItem in rewardItems)
            {
                if (!removeItems.ContainsKey(rewardItem.ItemId))
                {
                    removeItems.Add(rewardItem.ItemId, rewardItem.ItemNum);
                }
                else
                {
                    removeItems[rewardItem.ItemId] += rewardItem.ItemNum;
                }
            }

            UserInfoComponentS userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponentS>();
            foreach (KeyValuePair<int, int> pair in removeItems)
            {
                int itemConfigId = pair.Key;
                int leftNum = pair.Value;
                if (itemConfigId == 1)
                {
                    if (userInfoComponent.Gold < leftNum)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (itemConfigId == 2)
                {
                    if (userInfoComponent.Diamond < leftNum)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                foreach (Item item in self.Items.Values)
                {
                    if ((int)containerType == item.ContainerType && itemConfigId == item.ConfigId)
                    {
                        leftNum -= item.Num;
                    }
                }

                if (leftNum > 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static int RemoveItemData(this InventoryComponentS self, List<RewardItem> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
        {
            if (!self.HaveItemData(rewardItems, containerType))
            {
                return ErrorCode.ERR_NotEnoughItems;
            }

            for (int i = rewardItems.Count - 1; i >= 0; i--)
            {
                int itemConfigId = rewardItems[i].ItemId;
                int leftNum = rewardItems[i].ItemNum;

                if (!ItemConfigCategory.Instance.DataMap.ContainsKey(itemConfigId))
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemConfigId);

                if (itemConfigId <= 10000000)
                {
                    UserInfoComponentS userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponentS>();
                    switch (itemConfig.Id)
                    {
                        case ConfigData.Item_Gold:
                            userInfoComponent.ChangeRoleData(UserDataType.Gold, -leftNum);
                            break;
                        case ConfigData.Item_Diamond:
                            userInfoComponent.ChangeRoleData(UserDataType.Diamond, -leftNum);
                            break;
                    }

                    continue;
                }

                List<EntityRef<Item>> items = self.Items.Values.ToList();
                for (int j = items.Count - 1; j >= 0; j--)
                {
                    Item item = items[j];
                    if ((int)containerType == item.ContainerType && itemConfigId == item.ConfigId)
                    {
                        if (item.Num > 0)
                        {
                            if (item.Num > leftNum)
                            {
                                item.Num -= leftNum;
                                ItemNoticeHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Update);
                                break;
                            }
                            else
                            {
                                leftNum -= item.Num;
                                self.RemoveItem(item.Id);
                            }
                        }
                    }
                }
            }

            return ErrorCode.ERR_Success;
        }

        public static void AddItem(this InventoryComponentS self, Item item)
        {
            if (item.Parent != self)
            {
                self.AddChild(item);
            }

            if (self.Items.ContainsKey(item.Id))
            {
                return;
            }

            self.Items.Add(item.Id, item);
            ItemNoticeHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Add);
        }

        // 直接消耗掉
        public static bool RemoveItem(this InventoryComponentS self, long itemId)
        {
            if (!self.Items.TryGetValue(itemId, out EntityRef<Item> itemRef))
            {
                return false;
            }

            Item item = itemRef;
            self.Items.Remove(itemId);
            ItemNoticeHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Remove);
            item?.Dispose();

            return true;
        }

        public static bool RemoveItem(this InventoryComponentS self, long itemId, int num)
        {
            if (!self.Items.TryGetValue(itemId, out EntityRef<Item> itemRef))
            {
                return false;
            }

            Item item = itemRef;

            if (item.Num > num)
            {
                item.Num -= num;
                ItemNoticeHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Update);
                return true;
            }

            self.Items.Remove(itemId);
            ItemNoticeHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Remove);
            item?.Dispose();

            return true;
        }

        public static List<Item> GetAllItems(this InventoryComponentS self)
        {
            List<Item> items = new List<Item>();
            foreach (Item item in self.Items.Values)
            {
                items.Add(item);
            }

            return items;
        }

        public static List<Item> GetItemsByContainer(this InventoryComponentS self, InventoryContainerType containerType)
        {
            List<Item> items = new();
            foreach (Item item in self.Items.Values)
            {
                if (item.ContainerType != (int)containerType)
                {
                    continue;
                }

                items.Add(item);
            }

            return items;
        }

        public static List<Item> GetItemsByType(this InventoryComponentS self, ItemType type, InventoryContainerType containerType)
        {
            List<Item> items = new();
            foreach (Item item in self.Items.Values)
            {
                if (item.ContainerType != (int)containerType)
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                if (itemConfig.ItemType == (int)type)
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}