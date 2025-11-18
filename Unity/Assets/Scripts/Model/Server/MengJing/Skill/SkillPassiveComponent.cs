namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class SkillPassiveComponent : Entity, IAwake, IDestroy, ITransfer
    {
        public long Timer;
    }
}