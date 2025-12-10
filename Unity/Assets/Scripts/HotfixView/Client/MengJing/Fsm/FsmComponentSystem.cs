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

            // Move2DComponent moveComponent = unit.GetComponent<Move2DComponent>();
            // bool idle = moveComponent == null || moveComponent.IsArrived();
            // self.ChangeState(idle ? FsmStateEnum.FsmIdleState : FsmStateEnum.FsmRunState);

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

        public static void ChangeState(this FsmComponent self, int targetFsm, int skillId = 0)
        {
            Unit unit = self.GetParent<Unit>();
            SpineAnimator spineAnimator = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<SpineAnimator>();

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
                    spineAnimator?.Play(AnimName.Attack, false);
                    break;
                case FsmStateEnum.FsmIdleState:
                    spineAnimator?.Play(AnimName.Idle, true);
                    break;
                case FsmStateEnum.FsmRunState:
                    spineAnimator?.Play(AnimName.Run, true);
                    break;
                case FsmStateEnum.FsmSkillState:
                    spineAnimator?.Play(AnimName.Attack, false, true);
                    break;
                default:
                    break;
            }

            self.CurrentFsm = targetFsm;
        }
    }
}