using System.Collections.Generic;

namespace ET.Server
{
    public static class Function_Fight
    {
        public static void Fight(Unit attackUnit, Unit defendUnit, SkillS skill, float customActDamage = 0)
        {
            if (customActDamage == 0)
            {
                customActDamage = skill.SkillConfig.ActDamage;
            }

            FightInternal(attackUnit, defendUnit, skill.SkillConfig.DamageType, skill.SkillConfig.DamgeValue, skill.SkillConfig.SkillActType, customActDamage, skill.SkillConfig.Id);
            
            // 增加怒气
            attackUnit.GetComponent<NumericComponent>().ApplyChange(NumericType.Now_AngerValue, skill.SkillConfig.SkillAddAnger);
        }

        public static void Fight(Unit attackUnit, Unit defendUnit, BuffS buff)
        {
            FightInternal(attackUnit, defendUnit, buff.BuffConfig.DamageType, 0, SkillActType.Skill, buff.BuffConfig.DamagePro, buff.InitBuffData.SkillConfigId);
        }

        private static bool FightInternal(Unit attackUnit, Unit defendUnit, DamageType damageType, int damageValue, SkillActType skillActType, float customActDamage, int skillConfigId)
        {
            if (attackUnit == null)
            {
                return false;
            }

            if (defendUnit == null)
            {
                return false;
            }

            if (!attackUnit.IsCanAttackUnit(defendUnit))
            {
                return false;
            }

            //获取攻击方属性
            NumericComponent numericComponentAttack = attackUnit.GetComponent<NumericComponent>();
            long attack_Hp = numericComponentAttack.GetAsLong(NumericType.Now_Hp);
            long attack_MaxHp = numericComponentAttack.GetAsLong(NumericType.Now_MaxHp);
            long attack_MinAct = numericComponentAttack.GetAsLong(NumericType.Now_MinAct);
            long attack_MaxAct = numericComponentAttack.GetAsLong(NumericType.Now_MaxAct);
            long attack_MageAct = numericComponentAttack.GetAsLong(NumericType.Now_Mage);
            long attack_MinDef = numericComponentAttack.GetAsLong(NumericType.Now_MinDef);
            long attack_MaxDef = numericComponentAttack.GetAsLong(NumericType.Now_MaxDef);
            long attack_MinAdf = numericComponentAttack.GetAsLong(NumericType.Now_MinAdf);
            long attack_MaxAdf = numericComponentAttack.GetAsLong(NumericType.Now_MaxAdf);
            long attack_AtkDamageAddPro = numericComponentAttack.GetAsLong(NumericType.Now_AtkDamageAddPro);
            long attack_MageActAddPro = numericComponentAttack.GetAsLong(NumericType.Now_HitDamageLessPro);
            long attack_Cri = numericComponentAttack.GetAsLong(NumericType.Now_Cri);
            long attack_ReCri = numericComponentAttack.GetAsLong(NumericType.Now_ReCri);
            // ......

            //获取受击方属性
            NumericComponent numericComponentDefend = defendUnit.GetComponent<NumericComponent>();
            long defend_Hp = numericComponentDefend.GetAsLong(NumericType.Now_Hp);
            long defend_MaxHp = numericComponentDefend.GetAsLong(NumericType.Now_MaxHp);
            long defend_MinAct = numericComponentDefend.GetAsLong(NumericType.Now_MinAct);
            long defend_MaxAct = numericComponentDefend.GetAsLong(NumericType.Now_MaxAct);
            long defend_MageAct = numericComponentDefend.GetAsLong(NumericType.Now_Mage);
            long defend_MinDef = numericComponentDefend.GetAsLong(NumericType.Now_MinDef);
            long defend_MaxDef = numericComponentDefend.GetAsLong(NumericType.Now_MaxDef);
            long defend_MinAdf = numericComponentDefend.GetAsLong(NumericType.Now_MinAdf);
            long defend_MaxAdf = numericComponentDefend.GetAsLong(NumericType.Now_MaxAdf);
            long defend_AtkDamageAddPro = numericComponentDefend.GetAsLong(NumericType.Now_AtkDamageAddPro);
            long defend_MageActAddPro = numericComponentDefend.GetAsLong(NumericType.Now_HitDamageLessPro);
            long defend_Cri = numericComponentDefend.GetAsLong(NumericType.Now_Cri);
            long defend_ReCri = numericComponentDefend.GetAsLong(NumericType.Now_ReCri);
            // ...

            // 计算伤害
            long damage = 0;
            float actDamage = customActDamage;
            if (damageType == DamageType.Physical)
            {
                // 物理伤害
                long act = attack_MinAct < attack_MaxAct ? RandomHelper.NextLong(attack_MinAct, attack_MaxAct) : attack_MinAct;
                damage = (long)(act * actDamage) + damageValue;
                damageType = DamageType.Physical;

                // 免疫物理伤害
                if (defendUnit.GetComponent<StateComponentS>().StateTypeGet(StateType.PhysicalImmune))
                {
                    damage = 0;
                    damageType = DamageType.Immune;
                }
            }
            else
            {
                // 法术伤害
                damage = (long)(attack_MageAct * actDamage) + damageValue;
                damageType = DamageType.Magical;
            }

            // 伤害加成和伤害减免
            if (damage != 0)
            {
                damage = (long)(damage * (1 + (attack_AtkDamageAddPro / 10000f) - (defend_MageActAddPro / 10000f)));
            }

            // 暴击
            if (skillActType == SkillActType.Normal && RandomHelper.RandFloat01() <= (attack_Cri - defend_ReCri) / 10000f)
            {
                damage = damage * 2;
                damageType = DamageType.Critical;
            }

            // 无敌
            if (defendUnit.GetComponent<StateComponentS>().StateTypeGet(StateType.AllDamageImmune))
            {
                damage = 0;
                damageType = DamageType.Immune;
            }

            // 保护 免受伤害
            if (damage > 0 && numericComponentDefend.GetAsInt(NumericType.InvulnerableCount) > 0)
            {
                damage = 0;
                numericComponentDefend.ApplyChange(NumericType.InvulnerableCount, -1, false);
            }

            if (damage > 0)
            {
                // 链接技能
                string linkSkillHandler = "Buff_链接";
                BuffS buff = UnitHelper.HaveBuffByHandler(defendUnit, linkSkillHandler);
                if (buff != null)
                {
                    foreach (Unit u in defendUnit.GetParent<UnitComponent>().GetAll())
                    {
                        if (defendUnit.Id == u.Id)
                        {
                            continue;
                        }

                        if (!UnitHelper.IsTeam(defendUnit, u))
                        {
                            continue;
                        }

                        if (!attackUnit.IsCanAttackUnit(u))
                        {
                            continue;
                        }

                        if (UnitHelper.HaveBuffByHandler(u, linkSkillHandler) == null)
                        {
                            continue;
                        }

                        u.GetComponent<NumericComponent>().ApplyChange(NumericType.Now_Hp, (long)(-damage * buff.BuffConfig.BuffParameterValue / 10000f), true, false, attackUnit.Id, skillConfigId, damageType);
                    }
                }

                // 受到伤害触发被动
                defendUnit.GetComponent<SkillPassiveComponent>().OnTriggerPassiveSkill(SkillPassiveType.OnDamagedByChance, attackUnit.Id);
            }

            // 普通攻击触发被动
            if (skillActType == SkillActType.Normal)
            {
                attackUnit.GetComponent<SkillPassiveComponent>().OnTriggerPassiveSkill(SkillPassiveType.OnNormalAttackByChance, attackUnit.Id);
            }

            // 开始战斗触发被动
            attackUnit.GetComponent<SkillPassiveComponent>().OnTriggerPassiveSkill(SkillPassiveType.OnBattleStart);
            defendUnit.GetComponent<SkillPassiveComponent>().OnTriggerPassiveSkill(SkillPassiveType.OnBattleStart);

            // AI
            defendUnit.GetComponent<AIComponent>()?.BeAttack(attackUnit);

            // 结算伤害
            numericComponentDefend.ApplyChange(NumericType.Now_Hp, -damage, true, false, attackUnit.Id, skillConfigId, damageType);

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