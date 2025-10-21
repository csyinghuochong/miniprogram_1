namespace ET.Client
{
    public static class ClientInventoryHelper
    {
        public static async ETTask<int> GetAllItem(Scene root)
        {
            C2M_GetAllItem request = C2M_GetAllItem.Create();

            M2C_GetAllItem response = (M2C_GetAllItem)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            InventoryComponentC inventoryComponentC = root.GetComponent<InventoryComponentC>();
            inventoryComponentC.Clear();
            foreach (ItemInfo itemInfo in response.ItemList)
            {
                inventoryComponentC.AddItemFromMessage(itemInfo);
            }

            return response.Error;
        }

        public static async ETTask<int> SellItem(Scene root, long itemId)
        {
            C2M_SellItem request = C2M_SellItem.Create();

            M2C_SellItem response = (M2C_SellItem)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }
    }
}