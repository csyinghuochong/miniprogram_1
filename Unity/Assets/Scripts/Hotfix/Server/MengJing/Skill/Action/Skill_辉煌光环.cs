using System.Collections.Generic;

namespace ET.Server
{
    public class Skill_辉煌光环 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
        }

        public override void OnExecute(SkillS skill)
        {
            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Unit defendUnit = entities[i];

                if (!UnitHelper.IsTeam(skill.TheUnitFrom, defendUnit))
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