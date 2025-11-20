using System;

namespace ET.Server
{
    [EntitySystemOf(typeof(DeathTimeComponent))]
    [FriendOf(typeof(DeathTimeComponent))]
    public static partial class DeathTimeComponentSystem
    {
        [Invoke(TimerInvokeType.DeathTimer)]
        public class DeathTimer : ATimer<DeathTimeComponent>
        {
            protected override void Run(DeathTimeComponent self)
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
        private static void Awake(this DeathTimeComponent self, float args2)
        {
            self.LiveTime = args2;

            self.LastUpdateTime = TimeInfo.Instance.ClientNow();
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(1000, TimerInvokeType.DeathTimer, self);
        }

        [EntitySystem]
        private static void Destroy(this DeathTimeComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Update(this DeathTimeComponent self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;
            
            self.LiveTime -= deltaTime;
            
            if (self.LiveTime <= 0)
            {
                self.GetParent<Unit>().OnDead(null);
            }
        }
    }
}