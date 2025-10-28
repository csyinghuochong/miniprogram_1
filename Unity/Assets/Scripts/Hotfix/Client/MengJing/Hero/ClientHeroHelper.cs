using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    public static class ClientHeroHelper
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

            heroComponentC.Formation = response.Formation;

            return response.Error;
        }

        /// <summary>
        /// 设置英雄阵容
        /// </summary>
        /// <param name="root"></param>
        /// <param name="opType">0上阵 1下阵</param>
        /// <param name="heroId"></param>
        /// <param name="slotIndex"></param>
        /// <returns></returns>
        public static async ETTask<int> SetHeroFormation(Scene root, int opType, long heroId, int slotIndex)
        {
            C2M_SetHeroFormation request = C2M_SetHeroFormation.Create();
            request.OpType = opType;
            request.HeroId = heroId;
            request.SlotIndex = slotIndex;

            M2C_SetHeroFormation response = (M2C_SetHeroFormation)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return response.Error;
            }

            HeroComponentC heroComponentC = root.GetComponent<HeroComponentC>();
            heroComponentC.Formation = response.Formation;

            EventSystem.Instance.Publish(root, new HeroFormationUpdate());

            return response.Error;
        }

        public static async ETTask<int> SetHeroEquipment(Scene root, int opType, long heroId, long itemId)
        {
            C2M_SetHeroEquipment request = C2M_SetHeroEquipment.Create();
            request.OpType = opType;
            request.HeroId = heroId;
            request.ItemId = itemId;

            M2C_SetHeroEquipment response = (M2C_SetHeroEquipment)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static void Test_CreateMyHeroes(Scene root)
        {
            Unit hero_1 = UnitFactory.CreateHero(root.CurrentScene(), 10000001);
            hero_1.Position = new float3(0, 0, 0f);

            Unit hero_2 = UnitFactory.CreateHero(root.CurrentScene(), 10000002);
            hero_2.Position = new float3(-5f, 0, -10);

            Unit hero_3 = UnitFactory.CreateHero(root.CurrentScene(), 10000003);
            hero_3.Position = new float3(0, 0, -10);
            //
            // Unit hero_4 = UnitFactory.CreateHero(root.CurrentScene(), 10000003);
            // hero_4.Position = new float3(5f, 0, 0);
            //
            // Unit hero_5 = UnitFactory.CreateHero(root.CurrentScene(), 10000003);
            // hero_5.Position = new float3(0, 0, -5f);
        }

        public static void Test_CreateMonsters(Scene root)
        {
            Unit monster_1 = UnitFactory.CreateMonster(root.CurrentScene(), 10000001);
            monster_1.Position = new float3(-7f, 0, 5f);
            Unit monster_2 = UnitFactory.CreateMonster(root.CurrentScene(), 10000002);
            monster_2.Position = new float3(0, 0, 5f);
            Unit monster_3 = UnitFactory.CreateMonster(root.CurrentScene(), 10000003);
            monster_3.Position = new float3(7f, 0, 5f);
        }
    }
}