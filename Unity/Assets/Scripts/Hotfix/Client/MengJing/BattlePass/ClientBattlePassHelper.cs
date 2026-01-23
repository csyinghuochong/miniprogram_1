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
                BattlePassComponentC battlePassComponent = root.GetComponent<BattlePassComponentC>();
                battlePassComponent.Clear();
                foreach (BattlePassInfo battlePassInfo in response.BattlePassInfoList)
                {
                    battlePassComponent.AddOrUpdateBattlePass(battlePassInfo);
                }
            }

            return response.Error;
        }
        
        public static async ETTask<int> BattlePassGetAllReward(Scene root)
        {
            C2M_BattlePassGetAllReward request = C2M_BattlePassGetAllReward.Create();

            M2C_BattlePassGetAllReward response = (M2C_BattlePassGetAllReward)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                BattlePassComponentC battlePassComponent = root.GetComponent<BattlePassComponentC>();
                foreach (BattlePassInfo battlePassInfo in response.BattlePassInfoList)
                {
                    battlePassComponent.AddOrUpdateBattlePass(battlePassInfo);
                }
            }

            return response.Error;
        }
    }
}