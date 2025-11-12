using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// 给周围友方单位添加Buff
    /// DamageRange
    /// </summary>
    public class Skill_BuffFriendly : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
        }

        public override void OnExecute(SkillS skill)
        {
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Unit defendUnit = entities[i];

                if (!UnitHelper.IsTeam(skill.TheUnitFrom, defendUnit))
                {
                    continue;
                }

                // 直接距离判断
                if (math.distance(skill.TheUnitFrom.Position, defendUnit.Position) > skill.SkillConfig.DamageRange[0])
                {
                    continue;
                }

                for (int j = 0; j < skill.SkillConfig.BuffID.Length; j++)
                {
                    skill.SkillBuff(skill.SkillConfig.BuffID[j], defendUnit);
                }
            }

            skill.SkillState = SkillState.Finished;
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}