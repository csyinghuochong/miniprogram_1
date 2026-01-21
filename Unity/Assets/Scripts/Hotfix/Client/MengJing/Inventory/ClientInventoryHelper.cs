using System.Collections.Generic;

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

        public static async ETTask<int> MoveItem(Scene root, long itemId, InventoryContainerType containerType)
        {
            C2M_MoveItem request = C2M_MoveItem.Create();
            request.ItemIdList.Add(itemId);
            request.ContainerType = (int)containerType;

            M2C_MoveItem response = (M2C_MoveItem)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static async ETTask<int> ItemRecycle(Scene root, List<EntityRef<Item>> itemList)
        {
            C2M_ItemRecycle request = C2M_ItemRecycle.Create();
            foreach (Item item in itemList)
            {
                request.ItemIdList.Add(item.Id);
            }

            M2C_ItemRecycle response = (M2C_ItemRecycle)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static async ETTask<int> HeroRecycle(Scene root, List<EntityRef<Hero>> heroList)
        {
            C2M_HeroRecycle request = C2M_HeroRecycle.Create();
            foreach (Hero hero in heroList)
            {
                request.HeroIdList.Add(hero.Id);
            }

            M2C_HeroRecycle response = (M2C_HeroRecycle)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }
    }
}