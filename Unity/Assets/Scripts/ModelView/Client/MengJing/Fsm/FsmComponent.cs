namespace ET.Client
{
    public static class FsmStateEnum
    {
        public const int FsmNullState = 0;
        public const int FsmIdleState = 1;
        public const int FsmRunState = 2;
        public const int FsmDeathState = 3;
        public const int FsmSkillState = 4;
    }

    [ComponentOf(typeof(Unit))]
    public class FsmComponent : Entity, IAwake, IDestroy
    {
        public int CurrentFsm;
        public long Timer;
        public long WaitIdleTime;
        public string LastAnimator;
    }
}