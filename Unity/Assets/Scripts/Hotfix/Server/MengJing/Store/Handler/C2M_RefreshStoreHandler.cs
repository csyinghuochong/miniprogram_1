namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(StoreComponentS))]
    public class C2M_RefreshStoreHandler : MessageLocationHandler<Unit, C2M_RefreshStore, M2C_RefreshStore>
    {
        protected override async ETTask Run(Unit unit, C2M_RefreshStore request, M2C_RefreshStore response)
        {
            StoreComponentS storeComponent = unit.GetComponent<StoreComponentS>();

            if (storeComponent.RefreshNum <= 0)
            {
                response.Error = ErrorCode.ERR_StoreRefreshNumNotEnough;
                return;
            }

            InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();
            if (!inventoryComponent.HaveItemData(ConfigData.StoreRefreshCost))
            {
                response.Error = ErrorCode.ERR_NotEnoughItems;
                return;
            }

            storeComponent.RefreshStore();
            storeComponent.RefreshNum--;

            response.RefreshNum = storeComponent.RefreshNum;
            response.StoreItemList = storeComponent.StoreItemList;

            await ETTask.CompletedTask;
        }
    }
}