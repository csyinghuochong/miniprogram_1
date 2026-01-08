using System.Collections.Generic;

namespace ET.Server
{
    /// <summary>
    /// GameObjectParameter 5 出击次数
    /// </summary>
    public class Skill_无敌击: SkillHandler
    {
        public override void OnInit(Skill skill)
        {
        }

        public override void OnExecute(Skill skill)
        {
            foreach (int id in skill.SkillConfig.InitBuffID)
            {
                skill.SkillBuff(id, skill.TheUnitFrom);
            }
            
            int num = (int)skill.SkillConfig.GameObjectParameter[0];
            List<EntityRef<Unit>> entities = skill.TheUnitFrom.GetParent<UnitComponent>().GetAll();

            for (int i = 0; i < skill.SkillConfig.GameObjectParameter[0]; i++)
            {
                if (num <= 0)
                {
                    break;
                }
                
                for (int j = entities.Count - 1; j >= 0; j--)
                {
                    Unit defendUnit = entities[j];

                    if (!skill.TheUnitFrom.IsCanAttackUnit(defendUnit))
                    {
                        continue;
                    }

                    if (num <= 0)
                    {
                        break;
                    }

                    Function_Fight.Fight(skill.TheUnitFrom, defendUnit, skill);
                    num--;
                }
            }

            skill.SkillState = SkillState.Finished;
        }

        public override void OnUpdate(Skill skill, float deltaTime)
        {
        }

        public override void OnFinished(Skill skill)
        {
            
        }
    }
}