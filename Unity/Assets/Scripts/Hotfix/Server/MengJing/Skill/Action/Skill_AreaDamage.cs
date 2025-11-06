using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// 范围伤害
    /// DamageRangeType
    /// DamageRange
    /// </summary>
    public class Skill_AreaDamage : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            skill.ICheckShape = skill.CreateCheckShape(0);
        }

        public override void OnExecute(SkillS skill)
        {
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Unit uu = entities[i];

                if (!skill.ICheckShape.Contains(uu.Position))
                {
                    continue;
                }

                Function_Fight.Fight(skill.TheUnitFrom, skill.TheUnitTarget, skill);
            }

            skill.SkillState = SkillState.Finished;
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}