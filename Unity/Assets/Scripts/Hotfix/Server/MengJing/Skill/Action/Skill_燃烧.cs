using System.Collections.Generic;

namespace ET.Server
{
    public class Skill_燃烧 : SkillHandler
    {
        public override void OnInit(Skill skill)
        {
            skill.ICheckShape = skill.CreateCheckShape(0);
        }

        public override void OnExecute(Skill skill)
        {
        }

        public override void OnUpdate(Skill skill, float deltaTime)
        {
            skill.RunTime += deltaTime;

            if (skill.RunTime >= skill.SkillConfig.SkillLiveTime)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }

            skill.ICheckShape.s_position = skill.TheUnitFrom.Position;

            skill.TriggerTime -= deltaTime;
            if (skill.TriggerTime <= 0)
            {
                skill.TriggerTime = 1f;

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

        public override void OnFinished(Skill skill)
        {
        }
    }
}