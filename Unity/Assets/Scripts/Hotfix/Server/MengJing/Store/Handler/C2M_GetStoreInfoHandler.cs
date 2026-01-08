namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    [FriendOf(typeof(StoreComponent))]
    public class C2M_GetStoreInfoHandler : MessageLocationHandler<Unit, C2M_GetStoreInfo, M2C_GetStoreInfo>
    {
        protected override async ETTask Run(Unit unit, C2M_GetStoreInfo request, M2C_GetStoreInfo response)
        {
            StoreComponent storeComponent = unit.GetComponent<StoreComponent>();

            storeComponent.Check();
            response.RefreshTime = storeComponent.RefreshTime;
            response.RefreshNum = storeComponent.RefreshNum;
            response.StoreItemList = storeComponent.StoreItemList;

            await ETTask.CompletedTask;
        }
    }
}