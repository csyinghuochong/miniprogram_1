using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(StoreComponentS))]
    public class C2M_StoreBuyHandler : MessageLocationHandler<Unit, C2M_StoreBuy, M2C_StoreBuy>
    {
        protected override async ETTask Run(Unit unit, C2M_StoreBuy request, M2C_StoreBuy response)
        {
            using (await unit.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Store, unit.Id))
            {
                StoreComponentS storeComponent = unit.GetComponent<StoreComponentS>();
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

                InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();
                if (!inventoryComponent.HaveItemData(new List<RewardItem>() { new() { ItemId = storeItemConfig.SellType, ItemNum = storeItemConfig.SellValue } }))
                {
                    response.Error = ErrorCode.ERR_NotEnoughItems;
                    return;
                }
                
                storeComponent.StoreItemList[request.StoreItemId]--;

                inventoryComponent.RemoveItemData(new List<RewardItem>() { new() { ItemId = storeItemConfig.SellType, ItemNum = storeItemConfig.SellValue } });
                inventoryComponent.AddItemData(new List<RewardItem>() { new() { ItemId = storeItemConfig.SellType, ItemNum = storeItemConfig.SellValue } });
            }

            await ETTask.CompletedTask;
        }
    }
}