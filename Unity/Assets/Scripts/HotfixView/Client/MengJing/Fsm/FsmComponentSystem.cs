using System;
using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class MoveStart_PlayMoveAnimate : AEvent<Scene, MoveStart>
    {
        protected override async ETTask Run(Scene scene, MoveStart args)
        {
            Unit unit = args.Unit;

            unit.GetComponent<FsmComponent>()?.ChangeState(FsmStateEnum.FsmRunState);

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Current)]
    public class MoveStop_PlayIdleAnimate : AEvent<Scene, MoveStop>
    {
        protected override async ETTask Run(Scene scene, MoveStop args)
        {
            Unit unit = args.Unit;

            unit.GetComponent<FsmComponent>()?.ChangeState(FsmStateEnum.FsmIdleState);

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class Fsm_OnFsmChange : AEvent<Scene, FsmChange>
    {
        protected override async ETTask Run(Scene scene, FsmChange args)
        {
            args.Unit.GetComponent<FsmComponent>()?.ChangeState(args.FsmHandlerType, args.SkillId);

            await ETTask.CompletedTask;
        }
    }

    [FriendOf(typeof(FsmComponent))]
    [EntitySystemOf(typeof(FsmComponent))]
    public static partial class FsmComponentSystem
    {
        [Invoke(TimerInvokeType.FsmTimer)]
        public class FsmTimer : ATimer<FsmComponent>
        {
            protected override void Run(FsmComponent self)
            {
                try
                {
                    self.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        [EntitySystem]
        private static void Awake(this FsmComponent self)
        {
            Unit unit = self.GetParent<Unit>();

            self.SkeletonAnimation = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<SkeletonAnimation>();

            Move2DComponent moveComponent = unit.GetComponent<Move2DComponent>();
            bool idle = moveComponent == null || moveComponent.IsArrived();
            self.ChangeState(idle ? FsmStateEnum.FsmIdleState : FsmStateEnum.FsmRunState);

            self.WaitIdleTime = 0;
        }

        [EntitySystem]
        private static void Destroy(this FsmComponent self)
        {
            self.EndTimer();
        }

        private static void Update(this FsmComponent self)
        {
        }

        public static void EndTimer(this FsmComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        public static void BeginTimer(this FsmComponent self)
        {
            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.FsmTimer, self);
            }
        }

        public static void ChangeState(this FsmComponent self, int targetFsm, int skillid = 0)
        {
            Unit unit = self.GetParent<Unit>();

            switch (self.CurrentFsm)
            {
                case FsmStateEnum.FsmRunState:
                    break;
                case FsmStateEnum.FsmDeathState:
                    break;
                default:
                    break;
            }

            switch (targetFsm)
            {
                case FsmStateEnum.FsmDeathState:
                    // self.SkeletonAnimation.Skeleton.ScaleX = -1;  // X 轴缩放为 -1 实现翻转
                    self.SkeletonAnimation.AnimationState.SetAnimation(0, AnimationName.Death, false);
                    break;
                case FsmStateEnum.FsmIdleState:
                    self.SkeletonAnimation.AnimationState.SetAnimation(0, AnimationName.Idle, true);
                    break;
                case FsmStateEnum.FsmRunState:
                    var currentAnimation = self.SkeletonAnimation.AnimationState.GetCurrent(0);
                    if (currentAnimation == null || currentAnimation.Animation.Name != AnimationName.Run)
                    {
                        self.SkeletonAnimation.AnimationState.SetAnimation(0, AnimationName.Run, true);
                    }

                    break;
                case FsmStateEnum.FsmAttackState:
                    self.SkeletonAnimation.AnimationState.SetAnimation(0, AnimationName.Attack, true);
                    break;

                default:
                    break;
            }

            self.CurrentFsm = targetFsm;
        }
    }
}