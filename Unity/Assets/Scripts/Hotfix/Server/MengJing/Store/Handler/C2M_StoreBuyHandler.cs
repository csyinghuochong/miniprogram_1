using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(StoreComponent))]
    public class C2M_StoreBuyHandler : MessageLocationHandler<Unit, C2M_StoreBuy, M2C_StoreBuy>
    {
        protected override async ETTask Run(Unit unit, C2M_StoreBuy request, M2C_StoreBuy response)
        {
            using (await unit.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Store, unit.Id))
            {
                StoreComponent storeComponent = unit.GetComponent<StoreComponent>();
                if (!storeComponent.StoreItemList.ContainsKey(request.StoreItemId))
                {
                    response.Error = ErrorCode.ERR_StoreItemNotExist;
                    return;
                }

                if (storeComponent.StoreItemList[request.StoreItemId] <= 0)
                {
                    response.Error = ErrorCode.ERR_StoreItemNotEnough;
                    return;
                }

                StoreItemConfig storeItemConfig = StoreItemConfigCategory.Instance.Get(request.StoreItemId);

                InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();
                List<RewardItem> cost = new List<RewardItem>() { new() { ItemId = storeItemConfig.SellType, ItemNum = storeItemConfig.SellValue } };
                if (!inventoryComponent.HaveItemData(cost))
                {
                    response.Error = ErrorCode.ERR_NotEnoughItems;
                    return;
                }
                
                storeComponent.StoreItemList[request.StoreItemId]--;

                inventoryComponent.RemoveItemData(cost);
                inventoryComponent.AddItemData(new List<RewardItem>() { new() { ItemId = storeItemConfig.SellItemID, ItemNum = 1 } });
            }

            await ETTask.CompletedTask;
        }
    }
}