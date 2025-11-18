namespace ET.Server
{
    public class Skill_Common : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
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
            }
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}