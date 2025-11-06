namespace ET.Client
{
    public class SkillHandlerCAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [SkillHandlerC]
    public abstract class SkillHandlerC
    {
        public abstract void OnInit(SkillC skill);
        public abstract void OnExecute(SkillC skill);
        public abstract void OnUpdate(SkillC skill, float deltaTime);
        public abstract void OnFinished(SkillC skill);
    }
}