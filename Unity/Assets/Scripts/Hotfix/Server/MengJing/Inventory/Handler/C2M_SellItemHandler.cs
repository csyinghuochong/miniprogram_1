using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_SellItemHandler : MessageLocationHandler<Unit, C2M_SellItem, M2C_SellItem>
    {
        protected override async ETTask Run(Unit unit, C2M_SellItem request, M2C_SellItem response)
        {
            InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();

            Item item = inventoryComponent.GetItem(request.ItemId);

            if (item == null)
            {
                response.Error = ErrorCode.ERR_NotExistItem;
                return;
            }
            
            if (item.ContainerType != (int)InventoryContainerType.Bag)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            if (request.Num < 1 || request.Num > item.Num)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

            List<RewardItem> removeItems = new List<RewardItem>();
            removeItems.Add(new RewardItem() { ItemId = item.ConfigId, ItemNum = request.Num });
            int error = inventoryComponent.RemoveItemData(removeItems);
            if (error != ErrorCode.ERR_Success)
            {
                response.Error = error;
                return;
            }

            List<RewardItem> addItems = new List<RewardItem>();
            addItems.Add(new RewardItem() { ItemId = itemConfig.SellMoneyType, ItemNum = itemConfig.SellMoneyValue * request.Num });
            inventoryComponent.AddItemData(addItems);

            await ETTask.CompletedTask;
        }
    }
}