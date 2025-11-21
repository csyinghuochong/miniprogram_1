namespace ET.Server
{
    public class Skill_剑舞 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
        }

        public override void OnExecute(SkillS skill)
        {
            foreach (int id in skill.SkillConfig.InitBuffID)
            {
                skill.SkillBuff(id, skill.TheUnitFrom);
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