namespace ET.Client
{
    public class SkillHandlerCAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [SkillHandlerC]
    public abstract class SkillHandler
    {
        public abstract void OnInit(Skill skill, Unit theUnitFrom);
        public abstract void OnExecute(Skill skill);
        public abstract void OnUpdate(Skill skill);
        public abstract void OnFinished(Skill skill);
        public abstract void OnEffectLoaded(Skill skill);
        public virtual void OnTriggerEnter(Skill skill) { }
        public virtual void OnTriggerStay(Skill skill) { }
        public virtual void OnTriggerExit(Skill skill) { }
    }
}