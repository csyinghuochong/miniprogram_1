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
    }
}