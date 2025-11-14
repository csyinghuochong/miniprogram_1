using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(InventoryComponentC))]
    [FriendOf(typeof(InventoryComponentC))]
    public static partial class InventoryComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this InventoryComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this InventoryComponentC self)
        {
            foreach (var containerItems in self.ItemsByContainer.Values)
            {
                containerItems.Clear();
            }
            self.ItemsByContainer.Clear();
            self.ItemsByContainer = null;
        }

        /// <summary>
        /// 将道具添加到容器列表中
        /// </summary>
        private static void AddItemToContainer(this InventoryComponentC self, Item item)
        {
            if (!self.ItemsByContainer.TryGetValue(item.ContainerType, out List<EntityRef<Item>> containerItems))
            {
                containerItems = new List<EntityRef<Item>>();
                self.ItemsByContainer[item.ContainerType] = containerItems;
            }

            if (!containerItems.Contains(item))
            {
                containerItems.Add(item);
            }
        }

        /// <summary>
        /// 从容器列表中移除道具
        /// </summary>
        private static void RemoveItemFromContainer(this InventoryComponentC self, Item item)
        {
            if (self.ItemsByContainer.TryGetValue(item.ContainerType, out List<EntityRef<Item>> containerItems))
            {
                containerItems.Remove(item);
            }
        }

        /// <summary>
        /// 通过ItemId查找道具（遍历所有容器）
        /// </summary>
        public static Item GetItem(this InventoryComponentC self, long itemId)
        {
            foreach (var containerItems in self.ItemsByContainer.Values)
            {
                foreach (EntityRef<Item> itemRef in containerItems)
                {
                    Item item = itemRef;
                    if (item.Id == itemId)
                    {
                        return item;
                    }
                }
            }

            return null;
        }

        public static void AddItemFromMessage(this InventoryComponentC self, ItemInfo itemInfo)
        {
            Item item = self.AddChildWithId<Item>(itemInfo.Id);
            item.FromMessage(itemInfo);
            self.AddItemToContainer(item);
        }

        public static void RemoveItemById(this InventoryComponentC self, long itemId)
        {
            Item item = self.GetItem(itemId);
            if (item == null)
            {
                Log.Error($"itemId:{itemId} not found");
                return;
            }

            self.RemoveItemFromContainer(item);
            item?.Dispose();
        }

        public static void UpdateItem(this InventoryComponentC self, ItemInfo itemInfo)
        {
            Item item = self.GetItem(itemInfo.Id);
            if (item == null)
            {
                Log.Error($"itemId:{itemInfo.Id} not found");
                return;
            }

            if (item.ContainerType != itemInfo.ContainerType)
            {
                self.RemoveItemFromContainer(item);
                item.FromMessage(itemInfo);
                self.AddItemToContainer(item);
            }
            else
            {
                item.FromMessage(itemInfo);
            }
        }

        public static void Clear(this InventoryComponentC self)
        {
            foreach (var containerItems in self.ItemsByContainer.Values)
            {
                foreach (EntityRef<Item> itemRef in containerItems)
                {
                    Item item = itemRef;
                    item?.Dispose();
                }
                containerItems.Clear();
            }
            self.ItemsByContainer.Clear();
        }

        public static List<Item> GetAllItems(this InventoryComponentC self)
        {
            List<Item> items = new List<Item>();
            foreach (var containerItems in self.ItemsByContainer.Values)
            {
                foreach (EntityRef<Item> itemRef in containerItems)
                {
                    items.Add(itemRef);
                }
            }

            return items;
        }

        public static List<Item> GetItemsByContainer(this InventoryComponentC self, InventoryContainerType containerType)
        {
            List<Item> items = new();
            if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
            {
                foreach (EntityRef<Item> itemRef in containerItems)
                {
                    items.Add(itemRef);
                }
            }

            return items;
        }

        public static List<Item> GetItemsByType(this InventoryComponentC self, ItemType type, InventoryContainerType containerType)
        {
            List<Item> items = new();
            if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
            {
                foreach (EntityRef<Item> itemRef in containerItems)
                {
                    Item item = itemRef;
                    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                    if (itemConfig.ItemType == type)
                    {
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public static List<Item> GetItemsBySubType(this InventoryComponentC self, ItemType type, ItemSubType subType, InventoryContainerType containerType)
        {
            List<Item> items = new();
            if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
            {
                foreach (EntityRef<Item> itemRef in containerItems)
                {
                    Item item = itemRef;
                    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                    if (itemConfig.ItemType == type && itemConfig.ItemSubType == subType)
                    {
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        public static int GetItemNum(this InventoryComponentC self, int itemConfigId, InventoryContainerType containerType)
        {
            int num = 0;
            if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
            {
                foreach (EntityRef<Item> itemRef in containerItems)
                {
                    Item item = itemRef;
                    if (item.ConfigId == itemConfigId)
                    {
                        num+=item.Num;
                    }
                }
            }

            return num;
        }
    }
}
