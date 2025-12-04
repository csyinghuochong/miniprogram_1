namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(StoreComponentS))]
    public class C2M_GetStoreInfoHandler : MessageLocationHandler<Unit, C2M_GetStoreInfo, M2C_GetStoreInfo>
    {
        protected override async ETTask Run(Unit unit, C2M_GetStoreInfo request, M2C_GetStoreInfo response)
        {
            StoreComponentS storeComponent = unit.GetComponent<StoreComponentS>();

            response.LastRefreshTime = storeComponent.LastRefreshTime;
            response.StoreItemList = storeComponent.StoreItemList;

            await ETTask.CompletedTask;
        }
    }
}