namespace ET.Client
{
    public static class ClientRankHelper
    {
        public static async ETTask<int> GetAllRank(Scene root)
        {
            C2Rank_GetAllRank request = C2Rank_GetAllRank.Create();

            Rank2C_GetAllRank response = (Rank2C_GetAllRank)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                RankComponent rankComponent = root.GetComponent<RankComponent>();
                rankComponent.Clear();

                foreach (PlayerCombatPowerRankInfo rankDataInfo in response.PlayerCombatPowerRankInfoList)
                {
                    rankComponent.AddPlayerCombatPowerRankFromMessage(rankDataInfo);
                }

                foreach (AllianceRankInfo allianceRankInfo in response.AllianceRankInfoList)
                {
                    rankComponent.AddAllianceRankFromMessage(allianceRankInfo);
                }
            }

            return response.Error;
        }
    }
}