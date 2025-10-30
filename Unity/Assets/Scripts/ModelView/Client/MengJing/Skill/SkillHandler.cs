namespace ET.Client
{
    public class SkillHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [SkillHandler]
    public abstract class SkillHandler
    {
        public abstract void OnInit(SkillC skillC);
        public abstract void OnExecute(SkillC skillC);
        public abstract void OnUpdate(SkillC skillC);
        public abstract void OnFinished(SkillC skillC);
        public abstract void OnEffectLoaded(SkillC skillC);
    }
}