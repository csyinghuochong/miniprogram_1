using System.Collections.Generic;

namespace ET.Server
{
    public class Skill_治疗守卫 : SkillHandlerS
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

                    if (!UnitHelper.IsTeam(skill.TheUnitFrom, defendUnit))
                    {
                        continue;
                    }

                    if (skill.ICheckShape != null && !skill.ICheckShape.Contains(defendUnit.Position))
                    {
                        continue;
                    }

                    NumericComponentS numericComponent = defendUnit.GetComponent<NumericComponentS>();
                    long value = (long)(skill.SkillConfig.GameObjectParameter[0] * numericComponent.GetAsLong(NumericType.Now_MaxHp));
                    numericComponent.ApplyChange(NumericType.Now_Hp, value, true, true, skill.TheUnitFrom.Id, skill.SkillConfig.Id, DamageType.Recover);
                }
            }
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}