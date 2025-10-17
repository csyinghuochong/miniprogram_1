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
            self.Items.Clear();
            self.Items = null;
        }

        public static Item GetItem(this InventoryComponentC self, long itemId)
        {
            self.Items.TryGetValue(itemId, out EntityRef<Item> item);
            return item;
        }

        public static void AddItemFromMessage(this InventoryComponentC self, ItemInfo itemInfo)
        {
            Item item = self.AddChildWithId<Item>(itemInfo.Id);
            item.FromMessage(itemInfo);
            self.Items.Add(item.Id, item);

            EventSystem.Instance.Publish(self.Root(), new InventoryUpdate());
        }

        public static void RemoveItemById(this InventoryComponentC self, long itemId)
        {
            if (!self.Items.TryGetValue(itemId, out EntityRef<Item> itemRef))
            {
                Log.Error($"itemId:{itemId} not found");
                return;
            }

            Item item = itemRef;
            self.Items.Remove(itemId);
            item?.Dispose();

            EventSystem.Instance.Publish(self.Root(), new InventoryUpdate());
        }

        public static void UpdateItem(this InventoryComponentC self, ItemInfo itemInfo)
        {
            if (!self.Items.TryGetValue(itemInfo.Id, out EntityRef<Item> itemRef))
            {
                Log.Error($"itemId:{itemInfo.Id} not found");
                return;
            }

            Item item = itemRef;
            item.FromMessage(itemInfo);

            EventSystem.Instance.Publish(self.Root(), new InventoryUpdate());
        }

        public static void Clear(this InventoryComponentC self)
        {
            foreach (Item item in self.Items.Values)
            {
                item?.Dispose();
            }

            self.Items.Clear();
        }

        public static List<Item> GetAllItems(this InventoryComponentC self)
        {
            List<Item> items = new List<Item>();
            foreach (Item item in self.Items.Values)
            {
                items.Add(item);
            }

            return items;
        }

        public static List<Item> GetItemsByContainer(this InventoryComponentC self, InventoryContainerType containerType)
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

        public static List<Item> GetItemsByType(this InventoryComponentC self, ItemType type, InventoryContainerType containerType)
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