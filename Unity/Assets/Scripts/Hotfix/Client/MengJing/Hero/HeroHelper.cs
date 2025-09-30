namespace ET.Client
{
    public static class HeroHelper
    {
        public static async ETTask<int> GetAllHero(Scene root)
        {
            C2M_GetAllHero request = C2M_GetAllHero.Create();

            M2C_GetAllHero response = (M2C_GetAllHero)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            HeroComponentC heroComponentC = root.GetComponent<HeroComponentC>();
            heroComponentC.Clear();
            foreach (HeroInfo heroInfo in response.HeroList)
            {
                heroComponentC.AddHeroFromMessage(heroInfo);
            }

            return response.Error;
        }
    }
}