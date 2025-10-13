namespace ET.Client
{
    public class SkillHandlerAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [SkillHandler]
    public abstract class SkillHandler
    {
        public abstract void OnInit(Skill skill);
        public abstract void OnExecute(Skill skill);
        public abstract void OnUpdate(Skill skill);
        public abstract void OnFinished(Skill skill);
        public abstract void OnEffectLoaded(Skill skill);
    }
}