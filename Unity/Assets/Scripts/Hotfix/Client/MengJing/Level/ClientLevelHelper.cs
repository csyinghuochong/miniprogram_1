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
    }
}