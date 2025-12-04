namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(StoreComponentS))]
    public class C2M_RefreshStoreHandler : MessageLocationHandler<Unit, C2M_RefreshStore, M2C_RefreshStore>
    {
        protected override async ETTask Run(Unit unit, C2M_RefreshStore request, M2C_RefreshStore response)
        {
            StoreComponentS storeComponent = unit.GetComponent<StoreComponentS>();

            storeComponent.RefreshStore();

            response.LastRefreshTime = storeComponent.LastRefreshTime;
            response.StoreItemList = storeComponent.StoreItemList;

            await ETTask.CompletedTask;
        }
    }
}