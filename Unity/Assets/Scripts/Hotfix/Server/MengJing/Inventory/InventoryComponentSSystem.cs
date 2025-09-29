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

        public static List<Item> GetItems(this InventoryComponentS self)
        {
            List<Item> items = new List<Item>();
            foreach (Item item in self.Items.Values)
            {
                items.Add(item);
            }

            return items;
        }

        public static Item GetItem(this InventoryComponentS self, long itemId)
        {
            self.Items.TryGetValue(itemId, out EntityRef<Item> item);
            return item;
        }

        public static void AddItemData(this InventoryComponentS self, List<RewardItem> rewardItems, InventoryContainerType containerType)
        {
            for (int i = rewardItems.Count - 1; i >= 0; i--)
            {
                int itemConfigId = rewardItems[i].ItemId;
                int leftNum = rewardItems[i].ItemNum;
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemConfigId);

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

                if (leftNum <= 0)
                {
                    continue;
                }

                Item newItem = self.AddChild<Item>();
                newItem.ConfigId = itemConfigId;
                newItem.ContainerType = (int)containerType;
                newItem.Num = leftNum;
                self.AddItem(newItem);
            }
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

        public static List<Item> GetAllItems(this InventoryComponentS self)
        {
            List<Item> items = new List<Item>();
            foreach (Item item in self.Items.Values)
            {
                items.Add(item);
            }

            return items;
        }
    }
}