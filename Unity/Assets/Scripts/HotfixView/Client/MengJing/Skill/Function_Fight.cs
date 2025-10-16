using System.Collections.Generic;

namespace ET.Client
{
    public static class Function_Fight
    {
        /// <summary>
        /// 伤害计算
        /// </summary>
        /// <param name="attackUnit">攻击方</param>
        /// <param name="defendUnit">受击方</param>
        /// <param name="skill"></param>
        /// <returns></returns>
        public static bool Fight(Unit attackUnit, Unit defendUnit, Skill skill)
        {
            SkillConfig skillConfig = skill.SkillConfig;

            //获取攻击方属性
            NumericComponentC numericComponentAttack = attackUnit.GetComponent<NumericComponentC>();
            long attack_Hp = numericComponentAttack.GetAsLong(NumericType.Now_Hp);
            long attack_MaxHp = numericComponentAttack.GetAsLong(NumericType.Now_MaxHp);
            long attack_MinAct = numericComponentAttack.GetAsLong(NumericType.Now_MinAct);
            long attack_MaxAct = numericComponentAttack.GetAsLong(NumericType.Now_MaxAct);
            long attack_MageAct = numericComponentAttack.GetAsLong(NumericType.Now_Mage);
            long attack_MinDef = numericComponentAttack.GetAsLong(NumericType.Now_MinDef);
            long attack_MaxDef = numericComponentAttack.GetAsLong(NumericType.Now_MaxDef);
            long attack_MinAdf = numericComponentAttack.GetAsLong(NumericType.Now_MinAdf);
            long attack_MaxAdf = numericComponentAttack.GetAsLong(NumericType.Now_MaxAdf);
            // ......

            //获取受击方属性
            NumericComponentC numericComponentDefend = defendUnit.GetComponent<NumericComponentC>();
            long defend_Hp = numericComponentDefend.GetAsLong(NumericType.Now_Hp);
            long defend_MaxHp = numericComponentDefend.GetAsLong(NumericType.Now_MaxHp);
            long defend_MinAct = numericComponentDefend.GetAsLong(NumericType.Now_MinAct);
            long defend_MaxAct = numericComponentDefend.GetAsLong(NumericType.Now_MaxAct);
            long defend_MageAct = numericComponentDefend.GetAsLong(NumericType.Now_Mage);
            long defend_MinDef = numericComponentDefend.GetAsLong(NumericType.Now_MinDef);
            long defend_MaxDef = numericComponentDefend.GetAsLong(NumericType.Now_MaxDef);
            long defend_MinAdf = numericComponentDefend.GetAsLong(NumericType.Now_MinAdf);
            long defend_MaxAdf = numericComponentDefend.GetAsLong(NumericType.Now_MaxAdf);
            // ...

            // 计算伤害
            long damage = attack_MaxAct + skillConfig.DamgeValue - defend_MaxDef;
            if (damage <= 0)
            {
                return false;
            }

            // 结算伤害
            numericComponentDefend.ApplyChange(NumericType.Now_Hp, damage);

            return true;
        }

        // 更新角色的属性，把配置表、角色的各种等级、装备的属性等等计算后加到NumericComponentC中
        public static void UnitUpdateProperty_Base(Unit unit)
        {
            // 更新英雄属性
            if (unit.Type == UnitType.Hero)
            {
                HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);
                Hero hero = unit.Root().GetComponent<HeroComponentC>().GetHero(unit.Id);

                NumericComponentC numericComponent = unit.GetComponent<NumericComponentC>();
                numericComponent.ResetProperty();

                Dictionary<int, long> UpdateProDicList = new Dictionary<int, long>();

                int lv = hero.Lv;

                // 计算各种属性 比如 角色的基础属性、等级提升后加成属性、装备属性
                long heroBaseMaxHp = heroConfig.BaseHp + lv * 1;
                long heroBaseMinAct = heroConfig.BaseAct + lv * 1;

                // 汇总属性
                long baseMaxHp = heroBaseMaxHp;
                long baseMinAct = heroBaseMaxHp;

                // 保存基础属性数据
                AddUpdateProDicList(NumericType.Base_MaxHp_Base, baseMaxHp, UpdateProDicList);
                AddUpdateProDicList(NumericType.Base_MaxAct_Base, baseMaxHp, UpdateProDicList);

                //更新属性，设置到NumericComponent
                foreach (int key in UpdateProDicList.Keys)
                {
                    long setValue = numericComponent.GetAsLong(key) + UpdateProDicList[key];

                    long numType = key;
                    if (key > NumericType.Max)
                    {
                        numType = key / 100;
                    }

                    numericComponent.ApplyValue(key, setValue);
                }
            }
        }

        private static void AddUpdateProDicList(int typeID, long typeValue, Dictionary<int, long> dic)
        {
            //缓存属性
            if (dic.ContainsKey(typeID))
            {
                dic[typeID] += typeValue;
            }
            else
            {
                dic.Add(typeID, typeValue);
            }
        }
    }
}