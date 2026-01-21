using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ItemRecycleHandler : MessageLocationHandler<Unit, C2M_ItemRecycle, M2C_ItemRecycle>
    {
        protected override async ETTask Run(Unit unit, C2M_ItemRecycle request, M2C_ItemRecycle response)
        {
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            List<EntityRef<Item>> itemList = new List<EntityRef<Item>>();
            foreach (var itemId in request.ItemIdList)
            {
                Item item = inventoryComponent.GetItem(itemId);

                if (item == null)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                itemList.Add(item);
            }

            List<RewardItem> rewardItemList = CommonHelp.GetRecycleItems(itemList);

            List<long> itemIdList = new();
            foreach (Item item in itemList)
            {
                itemIdList.Add(item.Id);
            }

            inventoryComponent.RemoveItemList(itemIdList);

            inventoryComponent.AddItemData(rewardItemList);

            await ETTask.CompletedTask;
        }
    }
}