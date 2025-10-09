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

            heroComponentC.CurrentFormationIndex = response.CurrentFormationIndex;
            heroComponentC.Formation_1 = response.Formation_1;
            heroComponentC.Formation_2 = response.Formation_2;

            return response.Error;
        }

        public static async ETTask<int> SetHeroFormation(Scene root, int opType, long heroId, int formationIndex, int slotIndex)
        {
            C2M_SetHeroFormation request = C2M_SetHeroFormation.Create();
            request.OpType = opType;
            request.HeroId = heroId;
            request.FormationIndex = formationIndex;
            request.SlotIndex = slotIndex;

            M2C_SetHeroFormation response = (M2C_SetHeroFormation)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            HeroComponentC heroComponentC = root.GetComponent<HeroComponentC>();
            switch (formationIndex)
            {
                case 1:
                    heroComponentC.Formation_1 = response.Formation;
                    break;
                case 2:
                    heroComponentC.Formation_2 = response.Formation;
                    break;
            }

            return response.Error;
        }
    }
}