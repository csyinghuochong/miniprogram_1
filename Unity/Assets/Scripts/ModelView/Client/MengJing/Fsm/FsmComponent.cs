namespace ET.Client
{
    public enum FsmStateEnum
    {
        FsmNullState = 0,
        FsmIdleState = 1,
        FsmRunState = 2,
        FsmDeathState = 3,
        FsmSkillState = 4
    }

    [ComponentOf(typeof(Unit))]
    public class FsmComponent : Entity, IAwake, IDestroy
    {
        public SpineAnimator SpineAnimator;
        public FsmStateEnum CurrentFsm;
        public long Timer;
    }
}