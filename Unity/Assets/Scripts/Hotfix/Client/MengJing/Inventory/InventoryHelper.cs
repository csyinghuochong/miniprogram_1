namespace ET.Client
{
    public static class InventoryHelper
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
    }
}