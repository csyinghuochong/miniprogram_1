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
            foreach (var containerItems in self.ItemsByContainer.Values)
            {
                containerItems.Clear();
            }

            self.ItemsByContainer.Clear();
            self.ItemsByContainer = null;
        }

        [EntitySystem]
        private static void Deserialize(this InventoryComponentS self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Item item)
                {
                    self.AddItemToContainer(item);
                }
            }
        }

        /// <summary>
        /// 将道具添加到容器列表中（不同步到客户端）
        /// </summary>
        private static void AddItemToContainer(this InventoryComponentS self, Item item)
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
        /// 从容器列表中移除道具（不同步到客户端）
        /// </summary>
        private static void RemoveItemFromContainer(this InventoryComponentS self, Item item)
        {
            if (self.ItemsByContainer.TryGetValue(item.ContainerType, out List<EntityRef<Item>> containerItems))
            {
                containerItems.Remove(item);
            }
        }

        /// <summary>
        /// 通过ItemId查找道具（遍历所有容器）
        /// </summary>
        public static Item GetItem(this InventoryComponentS self, long itemId)
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

        /// <summary>
        /// 将道具移动到不同的容器，并同步到客户端
        /// </summary>
        public static void MoveItemToContainer(this InventoryComponentS self, Item item, InventoryContainerType targetContainer)
        {
            if (item == null)
            {
                return;
            }

            int oldContainerType = item.ContainerType;
            int newContainerType = (int)targetContainer;

            if (oldContainerType == newContainerType)
            {
                return;
            }

            self.RemoveItemFromContainer(item);

            item.ContainerType = newContainerType;

            self.AddItemToContainer(item);

            ItemHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Update);
        }

        public static int AddItemData(this InventoryComponentS self, List<RewardItem> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
        {
            // 创建批量同步消息
            M2C_ItemUpdateOp message = M2C_ItemUpdateOp.Create();

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

                // 从指定容器中查找可堆叠的道具
                if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
                {
                    foreach (EntityRef<Item> itemRef in containerItems)
                    {
                        Item item = itemRef;
                        if (itemConfigId == item.ConfigId)
                        {
                            if (item.Num < itemConfig.ItemPileSum)
                            {
                                if (item.Num + leftNum > itemConfig.ItemPileSum)
                                {
                                    leftNum = item.Num + leftNum - itemConfig.ItemPileSum;
                                    item.Num = itemConfig.ItemPileSum;
                                }
                                else
                                {
                                    item.Num = item.Num + leftNum;
                                    leftNum = 0;
                                }

                                message.ItemInfoUpdateList.Add(item.ToMessage());
                            }
                        }

                        if (leftNum <= 0)
                        {
                            break;
                        }
                    }
                }

                // 创建新道具
                while (leftNum > 0)
                {
                    Item newItem = self.AddChild<Item>();
                    newItem.ConfigId = itemConfigId;
                    newItem.ContainerType = (int)containerType;
                    ItemHelper.InitItem(newItem);

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

                    // 添加到容器
                    if (newItem.Parent != self)
                    {
                        self.AddChild(newItem);
                    }

                    self.AddItemToContainer(newItem);

                    message.ItemInfoAddList.Add(newItem.ToMessage());
                }
            }

            if (message.ItemInfoAddList.Count > 0 || message.ItemInfoUpdateList.Count > 0 || message.ItemInfoRemoveList.Count > 0)
            {
                MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
            }

            return ErrorCode.ERR_Success;
        }

        public static int AddItemData(this InventoryComponentS self, List<ItemInfo> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
        {
            return ErrorCode.ERR_Success;
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
                    if (userInfoComponent.GetGold() < leftNum)
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
                    if (userInfoComponent.GetDiamond() < leftNum)
                    {
                        return false;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
                {
                    foreach (EntityRef<Item> itemRef in containerItems)
                    {
                        Item item = itemRef;
                        if (itemConfigId == item.ConfigId)
                        {
                            leftNum -= item.Num;
                        }
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

            M2C_ItemUpdateOp message = M2C_ItemUpdateOp.Create();

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

                if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
                {
                    List<EntityRef<Item>> items = containerItems.ToList();
                    for (int j = items.Count - 1; j >= 0; j--)
                    {
                        Item item = items[j];
                        if (itemConfigId == item.ConfigId)
                        {
                            if (item.Num > 0)
                            {
                                if (item.Num > leftNum)
                                {
                                    item.Num -= leftNum;
                                    message.ItemInfoUpdateList.Add(item.ToMessage());
                                    break;
                                }
                                else
                                {
                                    leftNum -= item.Num;
                                    self.RemoveItemFromContainer(item);
                                    message.ItemInfoRemoveList.Add(item.ToMessage());
                                    item?.Dispose();
                                }
                            }
                        }
                    }
                }
            }

            // 批量发送
            if (message.ItemInfoAddList.Count > 0 || message.ItemInfoUpdateList.Count > 0 || message.ItemInfoRemoveList.Count > 0)
            {
                MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
            }

            return ErrorCode.ERR_Success;
        }

        public static void AddItem(this InventoryComponentS self, Item item)
        {
            if (item.Parent != self)
            {
                self.AddChild(item);
            }

            self.AddItemToContainer(item);
            ItemHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Add);
        }

        // 直接消耗掉
        public static bool RemoveItem(this InventoryComponentS self, long itemId)
        {
            Item item = self.GetItem(itemId);
            if (item == null)
            {
                return false;
            }

            self.RemoveItemFromContainer(item);
            ItemHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Remove);
            item?.Dispose();

            return true;
        }

        public static bool RemoveItem(this InventoryComponentS self, long itemId, int num)
        {
            Item item = self.GetItem(itemId);
            if (item == null)
            {
                return false;
            }

            if (item.Num > num)
            {
                item.Num -= num;
                ItemHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Update);
                return true;
            }

            self.RemoveItemFromContainer(item);
            ItemHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Remove);
            item?.Dispose();

            return true;
        }

        public static List<Item> GetAllItems(this InventoryComponentS self)
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

        public static List<Item> GetItemsByContainer(this InventoryComponentS self, InventoryContainerType containerType)
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

        public static List<Item> GetItemsByType(this InventoryComponentS self, ItemType type, InventoryContainerType containerType)
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
        
        public static int GetItemNum(this InventoryComponentS self, int itemConfigId, InventoryContainerType containerType = InventoryContainerType.Bag)
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