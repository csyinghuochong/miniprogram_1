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

        public static void UpdateHeroStats(Unit unit, Hero hero)
        {
            hero.NumericDic.Clear();

            // 英雄配置表属性
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            long base_MaxHp = heroConfig.BaseHp;
            long base_MinAct = heroConfig.BaseAct;
            long base_MaxAct = heroConfig.BaseAct;
            long base_MinDef = heroConfig.BaseDef;
            long base_MaxDef = heroConfig.BaseDef;
            long base_MinAdf = heroConfig.BaseAdf;
            long base_MaxAdf = heroConfig.BaseAdf;
            double base_Cri = heroConfig.BaseCri;
            double base_ReCri = heroConfig.BaseReCri;
            double base_Eva = heroConfig.BaseEva;
            double base_Hit = heroConfig.BaseHit;
            double base_HitLess = heroConfig.BaseHitLess;
            double base_MoveSpeed = heroConfig.BaseMoveSpeed;
            double base_AtkSpeed = heroConfig.BaseAtkSpeed;
            double base_Combo = 0;
            double base_Counterattack = 0;
            double base_LifeSteal = 0;
            double base_ReCombo = 0;
            double base_ReCounterattack = 0;
            double base_ReLifeSteal = 0;
            double base_ReEva = 0;
            long combatPower = 0;

            // 等级成长
            base_MaxHp += hero.Lv * heroConfig.LvHp;
            base_MinAct += hero.Lv * heroConfig.LvAct;
            base_MaxAct += hero.Lv * heroConfig.LvAct;
            base_MinDef += hero.Lv * heroConfig.LvDef;
            base_MaxDef += hero.Lv * heroConfig.LvDef;
            base_MinAdf += hero.Lv * heroConfig.LvAdf;
            base_MaxAdf += hero.Lv * heroConfig.LvAdf;

            // 装备
            InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();
            foreach (KeyValuePair<int, long> heroEquipment in hero.Equipments)
            {
                if (heroEquipment.Value != 0)
                {
                    Item item = inventoryComponent.GetItem(heroEquipment.Value);
                    if (item != null)
                    {
                        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
                        EquipConfig equipConfig = EquipConfigCategory.Instance.Get(itemConfig.ItemEquipID);

                        // 装备配置属性
                        base_MinAct += equipConfig.EquipMinAct;
                        base_MaxAct += equipConfig.EquipMaxAct;
                        base_MinDef += equipConfig.EquipMinDef;
                        base_MaxDef += equipConfig.EquipMaxDef;
                        base_MinAdf += equipConfig.EquipMinAdf;
                        base_MaxAdf += equipConfig.EquipMaxAdf;
                        base_MaxHp += equipConfig.EquipHp;
                        base_AtkSpeed += equipConfig.EquipAtkSpeed;
                        base_MoveSpeed += equipConfig.EquipMoveSpeed;
                        base_Cri += equipConfig.EquipCri;
                        base_Combo += equipConfig.EquipCombo;
                        base_Counterattack += equipConfig.EquipCounterattack;
                        base_LifeSteal += equipConfig.EquipLifeSteal;
                        base_Eva += equipConfig.EquipEva;
                        base_ReCri += equipConfig.EquipReCri;
                        base_ReCombo += equipConfig.EquipReCombo;
                        base_ReCounterattack += equipConfig.EquipReCounterattack;
                        base_ReLifeSteal += equipConfig.EquipLifeSteal;
                        base_ReEva += equipConfig.EquipReEva;
                    }
                }
            }

            // 计算战斗力
            combatPower = base_MaxHp + base_MinAct + base_MaxAct + base_MinDef + base_MaxDef + base_MinAdf + base_MaxAdf;

            // 保存数据
            hero.NumericDic.Add(NumericType.Now_Hp, base_MaxHp);
            hero.NumericDic.Add(NumericType.Base_MaxHp_Base, base_MaxHp);
            hero.NumericDic.Add(NumericType.Base_MinAct_Base, base_MinAct);
            hero.NumericDic.Add(NumericType.Base_MaxAct_Base, base_MaxAct);
            hero.NumericDic.Add(NumericType.Base_MinDef_Base, base_MinDef);
            hero.NumericDic.Add(NumericType.Base_MaxDef_Base, base_MaxDef);
            hero.NumericDic.Add(NumericType.Base_MinAdf_Base, base_MaxAdf);
            hero.NumericDic.Add(NumericType.Base_MaxAdf_Base, base_MinAdf);
            hero.NumericDic.Add(NumericType.Base_Cri_Base, (long)(base_Cri * 10000));
            hero.NumericDic.Add(NumericType.Base_ReCri_Base, (long)(base_ReCri * 10000));
            hero.NumericDic.Add(NumericType.Base_Eva_Base, (long)(base_Eva * 10000));
            hero.NumericDic.Add(NumericType.Base_Hit_Base, (long)(base_Hit * 10000));
            hero.NumericDic.Add(NumericType.Base_HitDamageLessPro_Base, (long)(base_HitLess * 10000));
            hero.NumericDic.Add(NumericType.Base_Speed_Base, (long)(base_MoveSpeed * 10000));
            hero.NumericDic.Add(NumericType.Base_AtkSpeed_Base, (long)(base_AtkSpeed * 10000));
            hero.NumericDic.Add(NumericType.CombatPower, combatPower);
        }
    }
}