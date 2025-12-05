namespace ET.Client
{
    public static class ClientStoreHelper
    {
        public static async ETTask<M2C_GetStoreInfo> GetStoreInfo(Scene root)
        {
            C2M_GetStoreInfo request = C2M_GetStoreInfo.Create();

            M2C_GetStoreInfo response = (M2C_GetStoreInfo)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response;
        }

        public static async ETTask<int> StoreBuy(Scene root, int id)
        {
            C2M_StoreBuy request = C2M_StoreBuy.Create();
            request.StoreItemId = id;

            M2C_StoreBuy response = (M2C_StoreBuy)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response.Error;
        }

        public static async ETTask<M2C_RefreshStore> RefreshStore(Scene root)
        {
            C2M_RefreshStore request = C2M_RefreshStore.Create();

            M2C_RefreshStore response = (M2C_RefreshStore)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response;
        }
    }
}