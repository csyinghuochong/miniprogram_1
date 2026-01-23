namespace ET.Client
{
    public static class ClientBattlePassHelper
    {
        public static async ETTask<int> GetAllBattlePass(Scene root)
        {
            C2M_GetAllBattlePass request = C2M_GetAllBattlePass.Create();

            M2C_GetAllBattlePass response = (M2C_GetAllBattlePass)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
            }

            return response.Error;
        }
    }
}