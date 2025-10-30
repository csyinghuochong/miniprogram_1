namespace ET.Client
{
    public class SkillHandlerCAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [SkillHandlerC]
    public abstract class SkillHandlerC
    {
        public abstract void OnInit(SkillC skillC);
        public abstract void OnExecute(SkillC skillC);
        public abstract void OnUpdate(SkillC skillC, float deltaTime);
        public abstract void OnFinished(SkillC skillC);
    }
}