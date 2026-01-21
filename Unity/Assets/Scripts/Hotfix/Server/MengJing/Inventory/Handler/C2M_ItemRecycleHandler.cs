using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ItemRecycleHandler : MessageLocationHandler<Unit, C2M_ItemRecycle, M2C_ItemRecycle>
    {
        protected override async ETTask Run(Unit unit, C2M_ItemRecycle request, M2C_ItemRecycle response)
        {
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            List<EntityRef<Item>> itemList = new();
            foreach (var itemId in request.ItemIdList)
            {
                Item item = inventoryComponent.GetItem(itemId, InventoryContainerType.Bag);

                if (item == null)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                itemList.Add(item);
            }

            List<RewardItem> rewardItemList = CommonHelp.GetRecycleItems(itemList);

            inventoryComponent.RemoveItemList(request.ItemIdList);

            inventoryComponent.AddItemData(rewardItemList);

            await ETTask.CompletedTask;
        }
    }
}