namespace ET.Client
{
    public static class InventoryHelper
    {
        public static async ETTask<int> GetAllItems(Scene root)
        {
            C2M_GetAllItem request = C2M_GetAllItem.Create();

            M2C_GetAllItem response = (M2C_GetAllItem)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            InventoryComponentC clientInventoryComponent = root.GetComponent<InventoryComponentC>();
            clientInventoryComponent.Clear();
            foreach (ItemInfo itemInfo in response.ItemList)
            {
                clientInventoryComponent.AddItemFromMessage(itemInfo);
            }

            return response.Error;
        }
    }
}