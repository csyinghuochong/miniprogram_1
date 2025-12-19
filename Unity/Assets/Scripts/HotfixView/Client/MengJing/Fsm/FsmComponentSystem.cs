using System;
using Cysharp.Text;
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

            self.SpineAnimator = unit.GetComponent<GameObjectComponent>().GameObject.GetComponent<SpineAnimator>();

            self.ChangeState(FsmStateEnum.FsmIdleState);
        }

        [EntitySystem]
        private static void Destroy(this FsmComponent self)
        {
            self.EndTimer();
        }

        private static void Update(this FsmComponent self)
        {
        }

        private static void EndTimer(this FsmComponent self)
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
            if (self.SpineAnimator == null)
            {
                Log.Error("SpineAnimator is null");
                return;
            }

            switch (self.CurrentFsm)
            {
                case FsmStateEnum.FsmIdleState:
                    break;
                case FsmStateEnum.FsmRunState:
                    break;
                case FsmStateEnum.FsmDeathState:
                    break;
                case FsmStateEnum.FsmSkillState:
                    break;
                default:
                    break;
            }

            switch (targetFsm)
            {
                case FsmStateEnum.FsmDeathState:
                    self.SpineAnimator.Play(AnimName.Attack, false);
                    break;
                case FsmStateEnum.FsmIdleState:
                    if (self.SpineAnimator.CurrentAnim != AnimName.Skill && self.SpineAnimator.CurrentAnim != AnimName.Attack)
                    {
                        self.SpineAnimator.Play(AnimName.Idle, true);
                    }

                    break;
                case FsmStateEnum.FsmRunState:
                    if (self.SpineAnimator.CurrentAnim != AnimName.Skill && self.SpineAnimator.CurrentAnim != AnimName.Attack)
                    {
                        self.SpineAnimator.Play(AnimName.Run, true);
                    }

                    break;
                case FsmStateEnum.FsmSkillState:
                    self.SpineAnimator.Play(AnimName.Attack, false, true);
                    break;
                default:
                    break;
            }

            self.CurrentFsm = targetFsm;
        }
    }
}