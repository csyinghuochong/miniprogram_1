namespace ET.Client
{
    public class Skill_Common : SkillHandlerC
    {
        public override void OnInit(SkillC skill)
        {
        }

        public override void OnExecute(SkillC skill)
        {
            skill.PlaySkillEffects(skill.TargetPosition);
        }

        public override void OnUpdate(SkillC skill, float deltaTime)
        {
            skill.RunTime += deltaTime;
            if (skill.RunTime >= skill.SkillConfig.SkillLiveTime)
            {
                skill.SkillState = SkillState.Finished;
            }
        }

        public override void OnFinished(SkillC skill)
        {
            skill.EndSkillEffect();
        }
    }
}