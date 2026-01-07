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
    }
}