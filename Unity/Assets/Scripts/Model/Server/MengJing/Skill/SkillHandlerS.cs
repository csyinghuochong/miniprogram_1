namespace ET.Server
{
    public class SkillHandlerSAttribute : BaseAttribute
    {
    }

    [EnableClass]
    [SkillHandlerSAttribute]
    public abstract class SkillHandlerS
    {
        public abstract void OnInit(SkillS skill);
        public abstract void OnExecute(SkillS skill);
        public abstract void OnUpdate(SkillS skill, float deltaTime);
        public abstract void OnFinished(SkillS skill);
    }
}