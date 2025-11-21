using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// 给周围单位添加Buff
    /// DamageRange
    /// </summary>
    public class Skill_守护天使 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            skill.ICheckShape = skill.CreateCheckShape(0);
        }

        public override void OnExecute(SkillS skill)
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

                if (!skill.ICheckShape.Contains(defendUnit.Position))
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

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}