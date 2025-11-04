using System;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current, NumericType.Now_Hp)]
    public class NumericWatcher_Hp_HitEffect : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            Unit unitDefend = args.Defend;
            Unit unitAttack = unitDefend.GetParent<UnitComponent>().Get(args.AttackId);

            EffectHelper.PlayHitEffect(unitAttack, unitDefend, args.SkillId);
        }
    }

    [Event(SceneType.Demo)]
    public class Skill_OnSkillEffect : AEvent<Scene, SkillEffect>
    {
        protected override async ETTask Run(Scene scene, SkillEffect args)
        {
            EffectViewComponent effectViewComponent = args.Unit.GetComponent<EffectViewComponent>();
            effectViewComponent?.EffectFactory(args.EffectData);

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(EffectViewComponent))]
    [FriendOf(typeof(EffectViewComponent))]
    public static partial class EffectViewComponentSystem
    {
        [Invoke(TimerInvokeType.EffectTimer)]
        public class EffectTimer : ATimer<EffectViewComponent>
        {
            protected override void Run(EffectViewComponent self)
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
        private static void Awake(this EffectViewComponent self)
        {
            self.InitEffect();
        }

        [EntitySystem]
        private static void Destroy(this EffectViewComponent self)
        {
            self.OnDispose();
        }

        private static void Update(this EffectViewComponent self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                Effect effect = self.Effects[i];

                if (effect.EffectState == EffectState.Finished)
                {
                    effect.Dispose();
                    self.Effects.RemoveAt(i);
                    continue;
                }

                effect.OnUpdate(deltaTime);
            }

            if (self.Effects.Count == 0)
            {
                self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
            }
        }

        public static void RemoveEffectId(this EffectViewComponent self, long instanceId)
        {
            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                if (self.Effects[i].EffectData.InstanceId == instanceId)
                {
                    self.Effects[i].EffectState = EffectState.Finished;
                }
            }
        }

        public static Effect GetEffect(this EffectViewComponent self, long instanceId)
        {
            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                if (self.Effects[i].EffectData.InstanceId == instanceId)
                {
                    return self.Effects[i];
                }
            }

            return null;
        }

        public static Effect EffectFactory(this EffectViewComponent self, EffectData effectData)
        {
            Unit unit = self.GetParent<Unit>();

            self.RemoveSameBuffEffect(effectData);

            Effect resultEffect = self.AddChild<Effect>(true);
            resultEffect.OnInit(effectData, unit);
            self.Effects.Add(resultEffect);

            if (self.Timer == 0)
            {
                self.LastUpdateTime = TimeInfo.Instance.ClientNow();
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.EffectTimer, self);
            }

            return resultEffect;
        }

        private static void InitEffect(this EffectViewComponent self)
        {
        }

        private static void RemoveSameBuffEffect(this EffectViewComponent self, EffectData effectData)
        {
        }

        public static void OnDispose(this EffectViewComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
        }
    }
}