using Spine.Unity;

namespace ET.Client
{
    public static class AnimationName
    {
        public const string Attack = "attack";
        public const string BeAttack = "beattack";
        public const string Death = "die";
        public const string Idle = "idle";
        public const string Magic = "magic";
        public const string Run = "run";
    }

    [ComponentOf(typeof(Unit))]
    public class FsmComponent : Entity, IAwake, IDestroy
    {
        public SkeletonAnimation SkeletonAnimation;

        public int CurrentFsm;
        public long Timer;
        public long WaitIdleTime;
        public string LastAnimator;
    }
}