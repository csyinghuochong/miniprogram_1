using System.Collections.Generic;

namespace ET.Server
{
    public static class HeroHelper
    {
        public static void SyncHeroInfo(Unit unit, Hero hero, HeroOpType heroOpType)
        {
            M2C_HeroUpdateOp m2CHeroUpdateOp = M2C_HeroUpdateOp.Create();
            m2CHeroUpdateOp.HeroInfo = hero.ToMessage();
            m2CHeroUpdateOp.HeroOpType = (int)heroOpType;
            MapMessageHelper.SendToClient(unit, m2CHeroUpdateOp);
        }

        public static void UpdateHeroNumeric(Unit unit, Hero hero)
        {
            hero.NumericDic.Clear();

            List<Item> equipments = new List<Item>();
            InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();
            foreach (KeyValuePair<int, long> heroEquipment in hero.Equipments)
            {
                if (heroEquipment.Value != 0)
                {
                    Item item = inventoryComponent.GetItem(heroEquipment.Value);
                    if (item != null)
                    {
                        equipments.Add(item);
                    }
                }
            }

            hero.NumericDic = CommonHelp.CalculateHeroNumeric(hero, equipments);
        }

        public static void AddHeroExp(Hero hero, int value)
        {
            hero.Exp += value;

            for (int i = 0; i < 99999; i++)
            {
                ExpConfig expConfig = ExpConfigCategory.Instance.Get(hero.Lv);

                if (hero.Exp < expConfig.HeroUpExp)
                {
                    break;
                }

                int nextLv = hero.Lv + 1;
                if (!ExpConfigCategory.Instance.DataMap.ContainsKey(nextLv) || ExpConfigCategory.Instance.Get(nextLv).HeroUpExp == 0)
                {
                    hero.Exp = expConfig.HeroUpExp;
                    break;
                }

                hero.Exp -= expConfig.HeroUpExp;
                hero.Lv += 1;
            }
        }

        public static void AddHeroHunShi(Hero hero, int value)
        {
            hero.HunShi += value;

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            for (int i = 0; i < 99999; i++)
            {
                if (hero.HunShi < heroConfig.HeroUpStarNeed[hero.Star])
                {
                    break;
                }

                int nextStar = hero.Star + 1;

                if (nextStar >= heroConfig.HeroUpStarNeed.Length)
                {
                    hero.HunShi = heroConfig.HeroUpStarNeed[hero.Star];
                    break;
                }

                hero.HunShi -= heroConfig.HeroUpStarNeed[hero.Star];
                hero.Star += 1;
            }
        }

        public static void UpdateHeroSkill(Hero hero)
        {
            hero.Skills.Clear();

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            
            hero.Skills.Add(heroConfig.AtkId);
            // foreach (UnlockSkillInfo info in heroConfig.UnlockSkillInfos)
            // {
            //     if (hero.Star >= info.UnlockStar)
            //     {
            //         hero.Skills.Add(info.SkillConfigId);
            //     }
            // }
            
            //先这样测试
            hero.Skills.Add(30000013);
        }
    }
}