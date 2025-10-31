using System.Collections.Generic;

namespace ET.Server
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
        public static bool Fight(Unit attackUnit, Unit defendUnit, SkillS skill)
        {
            SkillConfig skillConfig = skill.SkillConfig;

            //获取攻击方属性
            NumericComponentS numericComponentAttack = attackUnit.GetComponent<NumericComponentS>();
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
            NumericComponentS numericComponentDefend = defendUnit.GetComponent<NumericComponentS>();
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