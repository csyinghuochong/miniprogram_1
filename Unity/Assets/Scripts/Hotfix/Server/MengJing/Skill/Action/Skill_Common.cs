using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 通用技能处理器 - 配置驱动
    /// 通过配置表字段自动执行常见技能逻辑，支持以下模式：
    ///
    /// 1. 立即给自己加Buff (InitBuffID)
    /// 2. 立即给目标单体加Buff (SkillTargetType=TargetOnly + BuffID)
    /// 3. 立即给范围内敌人加Buff (DamageRangeType>0 + BuffID, 过滤敌人)
    /// 4. 立即给范围内友军加Buff (DamageRangeType>0 + BuffID, 过滤队友)
    /// 5. 立即AOE伤害 (DamageRangeType>0, 过滤敌人)
    /// 6. 立即AOE伤害+Buff (DamageRangeType>0 + BuffID, 过滤敌人)
    /// 7. 周期性AOE伤害 (SkillLiveTime>0 + GameObjectParameter[0]=间隔)
    /// 8. 空技能占位 (无任何效果)
    ///
    /// GameObjectParameter[0] - 周期触发间隔（秒）
    /// </summary>
    public class Skill_Common : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            // 如果有范围类型，创建碰撞检测体
            if (skill.SkillConfig.DamageRangeType > 0)
            {
                skill.ICheckShape = skill.CreateCheckShape(0);
            }

            // 如果有周期触发参数，初始化触发计时器
            if (skill.SkillConfig.GameObjectParameter != null && skill.SkillConfig.GameObjectParameter.Length > 0)
            {
                skill.TriggerTime = 0;
            }
        }

        public override void OnExecute(SkillS skill)
        {
            // 给自己加InitBuff
            if (skill.SkillConfig.InitBuffID != null && skill.SkillConfig.InitBuffID[0] != 0)
            {
                foreach (int id in skill.SkillConfig.InitBuffID)
                {
                    skill.SkillBuff(id, skill.TheUnitFrom);
                }
            }

            // 单体目标技能
            if (skill.SkillConfig.SkillTargetType == SkillTargetType.TargetOnly)
            {
                if (skill.TheUnitTarget == null)
                {
                    skill.SkillState = SkillState.Finished;
                    return;
                }

                if (skill.SkillConfig.BuffID != null && skill.SkillConfig.BuffID[0] != 0)
                {
                    foreach (int id in skill.SkillConfig.BuffID)
                    {
                        skill.SkillBuff(id, skill.TheUnitTarget);
                    }
                }

                if (skill.SkillConfig.ActDamage > 0 || skill.SkillConfig.DamgeValue > 0)
                {
                    Function_Fight.Fight(skill.TheUnitFrom, skill.TheUnitTarget, skill);
                }

                skill.SkillState = SkillState.Finished;
                return;
            }

            // AOE技能（范围伤害/范围Buff）
            if (skill.SkillConfig.DamageRangeType > 0)
            {
                // 如果有生命周期且有触发间隔，说明是周期性技能，不在Execute阶段触发
                if (skill.SkillConfig.SkillLiveTime > 0 &&
                    skill.SkillConfig.GameObjectParameter != null &&
                    skill.SkillConfig.GameObjectParameter.Length > 0)
                {
                    return;
                }

                ExecuteAOELogic(skill);
                skill.SkillState = SkillState.Finished;
                return;
            }

            // 全体队友
            if (skill.SkillConfig.SkillTargetType == SkillTargetType.AllTeam)
            {
                List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
                for (int i = entities.Count - 1; i >= 0; i--)
                {
                    Unit unit = entities[i];
                    if (!UnitHelper.IsTeam(skill.TheUnitFrom, unit))
                    {
                        continue;
                    }

                    foreach (int id in skill.SkillConfig.BuffID)
                    {
                        skill.SkillBuff(id, unit);
                    }
                }

                skill.SkillState = SkillState.Finished;
                return;
            }

            // 全体敌人
            if (skill.SkillConfig.SkillTargetType == SkillTargetType.AllEnemy)
            {
                List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
                for (int i = entities.Count - 1; i >= 0; i--)
                {
                    Unit unit = entities[i];
                    if (!skill.TheUnitFrom.IsCanAttackUnit(unit))
                    {
                        continue;
                    }

                    foreach (int id in skill.SkillConfig.BuffID)
                    {
                        skill.SkillBuff(id, unit);
                    }

                    if (skill.SkillConfig.ActDamage > 0 || skill.SkillConfig.DamgeValue > 0)
                    {
                        Function_Fight.Fight(skill.TheUnitFrom, unit, skill);
                    }
                }

                skill.SkillState = SkillState.Finished;
                return;
            }

            // 空技能（立即结束）
            skill.SkillState = SkillState.Finished;
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
            skill.RunTime += deltaTime;

            if (skill.RunTime >= skill.SkillConfig.SkillLiveTime)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }

            // 周期性触发逻辑
            if (skill.SkillConfig.GameObjectParameter != null &&
                skill.SkillConfig.GameObjectParameter.Length > 0 &&
                skill.SkillConfig.DamageRangeType > 0)
            {
                skill.TriggerTime -= deltaTime;
                if (skill.TriggerTime <= 0)
                {
                    skill.TriggerTime = skill.SkillConfig.GameObjectParameter[0];
                    ExecuteAOELogic(skill);
                }
            }
        }

        public override void OnFinished(SkillS skill)
        {
        }

        /// <summary>
        /// 执行AOE逻辑（伤害+Buff）
        /// </summary>
        private void ExecuteAOELogic(SkillS skill)
        {
            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();

            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Unit defendUnit = entities[i];

                if (defendUnit.Id == skill.TheUnitFrom.Id)
                {
                    continue;
                }

                if (skill.ICheckShape != null && !skill.ICheckShape.Contains(defendUnit.Position))
                {
                    continue;
                }

                if (skill.SkillConfig.BuffID.Length > 0 && skill.SkillConfig.BuffID[0] != 0)
                {
                    foreach (int id in skill.SkillConfig.BuffID)
                    {
                        skill.SkillBuff(id, defendUnit);
                    }
                }

                if (skill.TheUnitFrom.IsCanAttackUnit(defendUnit))
                {
                    if (skill.SkillConfig.ActDamage > 0 || skill.SkillConfig.DamgeValue > 0)
                    {
                        Function_Fight.Fight(skill.TheUnitFrom, defendUnit, skill);
                    }
                }
            }
        }
    }
}