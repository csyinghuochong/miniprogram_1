using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_MoveItemHandler : MessageLocationHandler<Unit, C2M_MoveItem, M2C_MoveItem>
    {
        protected override async ETTask Run(Unit unit, C2M_MoveItem request, M2C_MoveItem response)
        {
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            InventoryContainerType targetContainerType = (InventoryContainerType)request.ContainerType;
            if (targetContainerType != InventoryContainerType.Bag && targetContainerType != InventoryContainerType.Warehouse)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            List<Item> moveItems = new();
            foreach (long id in request.ItemIdList)
            {
                Item item = inventoryComponent.GetItem(id);
                if (item == null)
                {
                    response.Error = ErrorCode.ERR_NotExistItem;
                    return;
                }

                if (item.ContainerType != (int)InventoryContainerType.Bag && item.ContainerType != (int)InventoryContainerType.Warehouse)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                moveItems.Add(item);
            }

            inventoryComponent.MoveItemToContainer(moveItems, targetContainerType);

            await ETTask.CompletedTask;
        }
    }
}