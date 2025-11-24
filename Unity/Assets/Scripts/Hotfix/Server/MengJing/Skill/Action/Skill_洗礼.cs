using System.Collections.Generic;

namespace ET.Server
{
    public class Skill_洗礼 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            skill.ICheckShape = skill.CreateCheckShape(0);
        }

        public override void OnExecute(SkillS skill)
        {
            bool triggerTeam = false;
            
            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Unit defendUnit = entities[i];

                if (!triggerTeam && UnitHelper.IsTeam(skill.TheUnitFrom, defendUnit))
                {
                    foreach (int id in skill.SkillConfig.BuffID)
                    {
                        skill.SkillBuff(id, defendUnit);
                    }

                    triggerTeam = true;
                }

                if (!skill.TheUnitFrom.IsCanAttackUnit(defendUnit))
                {
                    continue;
                }

                if (!skill.ICheckShape.Contains(defendUnit.Position))
                {
                    continue;
                }

                Function_Fight.Fight(skill.TheUnitFrom, defendUnit, skill);
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