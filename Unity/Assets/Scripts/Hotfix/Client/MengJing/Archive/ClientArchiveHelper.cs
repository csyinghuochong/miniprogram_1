namespace ET.Client
{
    public static class ClientArchiveHelper
    {
        public static async ETTask<int> GetAllArchiveHero(Scene root)
        {
            C2M_GetAllArchiveHero request = C2M_GetAllArchiveHero.Create();

            M2C_GetAllArchiveHero response = (M2C_GetAllArchiveHero)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                ArchiveComponentC archiveComponent = root.GetComponent<ArchiveComponentC>();
                archiveComponent.Clear();
                archiveComponent.ReceivedArchiveRewardIds.AddRange(response.ReceivedArchiveRewardIds);
                foreach (ArchiveHeroInfo archiveHeroInfo in response.ArchiveHeroInfoList)
                {
                    archiveComponent.AddOrUpdateArchiveHero(archiveHeroInfo);
                }
            }

            return response.Error;
        }

        public static async ETTask<int> ActiveArchiveHero(Scene root, int heroConfigId)
        {
            C2M_ActiveArchiveHero request = C2M_ActiveArchiveHero.Create();
            request.HeroConfigId = heroConfigId;

            M2C_ActiveArchiveHero response = (M2C_ActiveArchiveHero)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static async ETTask<int> ReceivedArchiveReward(Scene root, int rewardId)
        {
            C2M_ReceivedArchiveReward request = C2M_ReceivedArchiveReward.Create();
            request.RewardId = rewardId;

            M2C_ReceivedArchiveReward response = (M2C_ReceivedArchiveReward)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                ArchiveComponentC archiveComponent = root.GetComponent<ArchiveComponentC>();
                archiveComponent.ReceivedArchiveRewardIds.Add(rewardId);
            }

            return response.Error;
        }
    }
}