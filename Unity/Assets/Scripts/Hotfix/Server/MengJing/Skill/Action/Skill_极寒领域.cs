using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// GameObjectParameter 10000 触发时间间隔
    /// </summary>
    public class Skill_极寒领域 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            skill.ICheckShape = skill.CreateCheckShape(0);
            skill.TriggerTime = 0;
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

            skill.TriggerTime -= deltaTime;
            if (skill.TriggerTime >= 0)
            {
                return;
            }
            skill.TriggerTime = skill.SkillConfig.GameObjectParameter[0] / 10000f;
            
            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                Unit defendUnit = entities[i];
                
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
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}