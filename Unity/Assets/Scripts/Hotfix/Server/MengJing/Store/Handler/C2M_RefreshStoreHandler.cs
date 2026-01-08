namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(StoreComponent))]
    public class C2M_RefreshStoreHandler : MessageLocationHandler<Unit, C2M_RefreshStore, M2C_RefreshStore>
    {
        protected override async ETTask Run(Unit unit, C2M_RefreshStore request, M2C_RefreshStore response)
        {
            StoreComponent storeComponent = unit.GetComponent<StoreComponent>();

            if (storeComponent.RefreshNum <= 0)
            {
                response.Error = ErrorCode.ERR_StoreRefreshNumNotEnough;
                return;
            }

            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();
            if (!inventoryComponent.HaveItemData(ConfigData.StoreRefreshCost))
            {
                response.Error = ErrorCode.ERR_NotEnoughItems;
                return;
            }
            
            inventoryComponent.RemoveItemData(ConfigData.StoreRefreshCost);

            storeComponent.RefreshStore();
            storeComponent.RefreshNum--;

            response.RefreshNum = storeComponent.RefreshNum;
            response.StoreItemList = storeComponent.StoreItemList;

            await ETTask.CompletedTask;
        }
    }
}