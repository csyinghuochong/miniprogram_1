using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(BuffManagerComponentC))]
    [FriendOf(typeof(BuffManagerComponentC))]
    public static partial class BuffManagerComponentCSystem
    {
        [Invoke(TimerInvokeType.BuffTimerC)]
        public class BuffTimerC : ATimer<BuffManagerComponentC>
        {
            protected override void Run(BuffManagerComponentC self)
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
        private static void Awake(this BuffManagerComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BuffManagerComponentC self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
            self.Buffs.Clear();
        }

        private static void Update(this BuffManagerComponentC self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buff = self.Buffs[i];

                if (buff.BuffState == BuffState.Finished)
                {
                    buff.OnFinished();
                    buff.Dispose();
                    self.Buffs.RemoveAt(i);
                    continue;
                }

                buff.OnUpdate(deltaTime);
            }

            if (self.Buffs.Count == 0)
            {
                self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
            }
        }

        public static void BuffFactory(this BuffManagerComponentC self, BuffData buffData)
        {
            BuffC buff = self.AddChild<BuffC>();
            buff.OnInit(buffData, self.GetParent<Unit>());
            self.Buffs.Add(buff);

            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(500, TimerInvokeType.BuffTimerC, self);
            }
        }

        public static void RemoveBuff(this BuffManagerComponentC self, int buffConfigId)
        {
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buff = self.Buffs[i];

                if (buff.BuffData.BuffConfigId == buffConfigId)
                {
                    buff.BuffState = BuffState.Finished;
                }
            }
        }

        public static int GetBuffNumber(this BuffManagerComponentC self, int buffConfigId)
        {
            int number = 0;
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buff = self.Buffs[i];
                if (buff.BuffData.BuffConfigId == buffConfigId)
                {
                    number++;
                }
            }

            return number;
        }

        public static int GetBuffSourceNumber(this BuffManagerComponentC self, long formId, int buffConfigId)
        {
            int buffNumber = 0;

            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buff = self.Buffs[i];
                if (buff.BuffData.BuffConfigId != buffConfigId)
                {
                    continue;
                }

                if (formId != 0 && formId != buff.BuffData.UnitIdFrom)
                {
                    continue;
                }

                buffNumber++;
            }

            return buffNumber;
        }

        public static List<BuffC> GetBuffByConfigId(this BuffManagerComponentC self, int buffConfigId)
        {
            List<BuffC> list = new List<BuffC>();
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buff = self.Buffs[i];

                if (buff.BuffConfig.Id == buffConfigId)
                {
                    list.Add(buff);
                }
            }

            return list;
        }

        public static void OnDead(this BuffManagerComponentC self)
        {
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buff = self.Buffs[i];

                buff.BuffState = BuffState.Finished;
            }
        }
    }
}