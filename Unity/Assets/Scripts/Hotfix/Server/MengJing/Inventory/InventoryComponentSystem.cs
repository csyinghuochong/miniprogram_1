using System.Collections.Generic;
using System.Linq;

namespace ET.Server
{
    [EntitySystemOf(typeof(InventoryComponent))]
    [FriendOf(typeof(InventoryComponent))]
    public static partial class InventoryComponentSystem
    {
        [EntitySystem]
        private static void Awake(this InventoryComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this InventoryComponent self)
        {
            foreach (var containerItems in self.ItemsByContainer.Values)
            {
                containerItems.Clear();
            }

            self.ItemsByContainer.Clear();
            self.ItemsByContainer = null;
        }

        [EntitySystem]
        private static void Deserialize(this InventoryComponent self)
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
        private static void AddItemToContainer(this InventoryComponent self, Item item)
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
        private static void RemoveItemFromContainer(this InventoryComponent self, Item item)
        {
            if (self.ItemsByContainer.TryGetValue(item.ContainerType, out List<EntityRef<Item>> containerItems))
            {
                containerItems.Remove(item);
            }
        }

        /// <summary>
        /// 处理特殊道具(金币、钻石、经验等),返回true表示已处理,false表示是普通道具
        /// </summary>
        private static bool TryAddSpecialItem(this InventoryComponent self, int itemConfigId, int num)
        {
            if (itemConfigId > 10000000)
            {
                return false;
            }

            UserInfoComponent userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponent>();

            switch (itemConfigId)
            {
                case ConfigData.Item_Gold:
                    userInfoComponent.ChangeRoleData(UserDataType.Gold, num);
                    break;
                case ConfigData.Item_Diamond:
                    userInfoComponent.ChangeRoleData(UserDataType.Diamond, num);
                    break;
                case ConfigData.Item_Exp:
                    userInfoComponent.ChangeRoleData(UserDataType.Exp, num);
                    break;
            }

            return true;
        }

        /// <summary>
        /// 通过ItemId查找道具（遍历所有容器）
        /// </summary>
        public static Item GetItem(this InventoryComponent self, long itemId)
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

        public static Item GetItem(this InventoryComponent self, long itemId, InventoryContainerType containerType)
        {
            if (self.ItemsByContainer.TryGetValue((int)containerType, out List<EntityRef<Item>> containerItems))
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
        public static void MoveItemToContainer(this InventoryComponent self, Item item, InventoryContainerType targetContainer)
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

        /// <summary>
        /// 将道具移动到不同的容器，并同步到客户端
        /// </summary>
        public static void MoveItemToContainer(this InventoryComponent self, List<Item> items, InventoryContainerType targetContainer)
        {
            if (items == null || items.Count == 0)
            {
                return;
            }

            M2C_ItemUpdateOp message = M2C_ItemUpdateOp.Create();

            foreach (Item item in items)
            {
                int oldContainerType = item.ContainerType;
                int newContainerType = (int)targetContainer;

                if (oldContainerType == newContainerType)
                {
                    continue;
                }

                self.RemoveItemFromContainer(item);

                item.ContainerType = newContainerType;

                self.AddItemToContainer(item);
                
                message.ItemInfoUpdateList.Add(item.ToMessage());
            }

            MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
        }

        public static int AddItemData(this InventoryComponent self, List<RewardItem> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
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

                // 处理特殊道具(金币、钻石、经验等)
                if (self.TryAddSpecialItem(itemConfigId, leftNum))
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemConfigId);

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

        public static int AddItemData(this InventoryComponent self, List<ItemInfo> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
        {
            // 创建批量同步消息
            M2C_ItemUpdateOp message = M2C_ItemUpdateOp.Create();

            for (int i = rewardItems.Count - 1; i >= 0; i--)
            {
                ItemInfo itemInfo = rewardItems[i];
                int itemConfigId = itemInfo.ConfigId;
                int leftNum = itemInfo.Num;

                if (!ItemConfigCategory.Instance.DataMap.ContainsKey(itemConfigId))
                {
                    continue;
                }

                // 处理特殊道具(金币、钻石、经验等)
                if (self.TryAddSpecialItem(itemConfigId, leftNum))
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemConfigId);

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
                    // ItemHelper.InitItem(newItem);

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

            // 批量发送同步消息
            if (message.ItemInfoAddList.Count > 0 || message.ItemInfoUpdateList.Count > 0 || message.ItemInfoRemoveList.Count > 0)
            {
                MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
            }

            return ErrorCode.ERR_Success;
        }
        
        public static bool HaveItemData(this InventoryComponent self, List<RewardItem> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
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

            UserInfoComponent userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponent>();
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

        public static int RemoveItemData(this InventoryComponent self, List<RewardItem> rewardItems, InventoryContainerType containerType = InventoryContainerType.Bag)
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
                    UserInfoComponent userInfoComponent = self.GetParent<Unit>().GetComponent<UserInfoComponent>();
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

        public static void AddItem(this InventoryComponent self, Item item)
        {
            if (item.Parent != self)
            {
                self.AddChild(item);
            }

            self.AddItemToContainer(item);
            ItemHelper.SyncItemInfo(self.GetParent<Unit>(), item, ItemOpType.Add);
        }

        // 直接消耗掉
        public static bool RemoveItem(this InventoryComponent self, long itemId)
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

        public static void RemoveItemList(this InventoryComponent self, List<long> itemIdList)
        {
            M2C_ItemUpdateOp message = M2C_ItemUpdateOp.Create();

            foreach (long itemId in itemIdList)
            {
                Item item = self.GetItem(itemId);

                if (item != null)
                {
                    message.ItemInfoRemoveList.Add(item.ToMessage());

                    self.RemoveItemFromContainer(item);
                    item?.Dispose();
                }
            }

            MapMessageHelper.SendToClient(self.GetParent<Unit>(), message);
        }

        public static bool RemoveItem(this InventoryComponent self, long itemId, int num)
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

        public static List<Item> GetAllItems(this InventoryComponent self)
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

        public static List<Item> GetItemsByContainer(this InventoryComponent self, InventoryContainerType containerType)
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

        public static List<Item> GetItemsByType(this InventoryComponent self, ItemType type, InventoryContainerType containerType)
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
        
        public static int GetItemNum(this InventoryComponent self, int itemConfigId, InventoryContainerType containerType = InventoryContainerType.Bag)
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