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

        public static async ETTask<int> SellItem(Scene root, long itemId, int num)
        {
            C2M_SellItem request = C2M_SellItem.Create();
            request.ItemId = itemId;
            request.Num = num;

            M2C_SellItem response = (M2C_SellItem)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static async ETTask<int> UseItem(Scene root, long itemId, int num = 1, long heroId = 0)
        {
            C2M_UseItem request = C2M_UseItem.Create();
            request.ItemId = itemId;
            request.Num = num;
            request.HeroId = heroId;

            M2C_UseItem response = (M2C_UseItem)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }
    }
}