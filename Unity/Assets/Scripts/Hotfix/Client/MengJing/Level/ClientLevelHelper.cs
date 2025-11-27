namespace ET.Client
{
    public static class ClientLevelHelper
    {
        public static async ETTask<int> EnterBossRoom(Scene root)
        {
            C2M_EnterBossRoom request = C2M_EnterBossRoom.Create();

            M2C_EnterBossRoom response = (M2C_EnterBossRoom)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static async ETTask<int> SetAutoFight(Scene root, bool value)
        {
            C2M_SetAutoFight request = C2M_SetAutoFight.Create();
            request.Value = value ? 1 : 0;

            M2C_SetAutoFight response = (M2C_SetAutoFight)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static async ETTask<int> PickUpDropItem(Scene root)
        {
            C2M_PickUpDropItem request = C2M_PickUpDropItem.Create();

            foreach (Unit unit in root.CurrentScene().GetComponent<UnitComponent>().GetAll())
            {
                if (unit.Type == UnitType.DropItem)
                {
                    request.UnitIdList.Add(unit.Id);
                }
            }

            M2C_PickUpDropItem response = (M2C_PickUpDropItem)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }
    }
}