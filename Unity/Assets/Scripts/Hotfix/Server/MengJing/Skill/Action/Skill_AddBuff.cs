using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// 给周围单位添加Buff
    /// DamageRange
    /// </summary>
    public class Skill_AddBuff : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
        }

        public override void OnExecute(SkillS skill)
        {
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
            foreach (int id in skill.SkillConfig.InitBuffID)
            {
                skill.SkillBuff(id, skill.TheUnitFrom);
            }

            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Unit defendUnit = entities[i];

                if (defendUnit.Id == skill.TheUnitFrom.Id)
                {
                    continue;
                }

                // 直接距离判断
                if (math.distance(skill.TheUnitFrom.Position, defendUnit.Position) > skill.SkillConfig.DamageRange[0])
                {
                    continue;
                }

                foreach (int id in skill.SkillConfig.BuffID)
                {
                    skill.SkillBuff(id, defendUnit);
                }
            }

            skill.SkillState = SkillState.Finished;
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}