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
    }
}