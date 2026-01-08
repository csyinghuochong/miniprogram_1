using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GetAllItemHandler : MessageLocationHandler<Unit, C2M_GetAllItem, M2C_GetAllItem>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllItem request, M2C_GetAllItem response)
        {
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            foreach (var item in inventoryComponent.GetAllItems())
            {
                response.ItemList.Add(item.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}