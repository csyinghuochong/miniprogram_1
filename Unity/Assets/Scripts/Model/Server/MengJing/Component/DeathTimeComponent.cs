namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class DeathTimeComponent : Entity, IAwake<float>, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public float LiveTime;
    }
}