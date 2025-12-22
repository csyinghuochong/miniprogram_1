using System;
using System.Collections.Generic;
using Unity.Mathematics;

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
            self.Timer = 0;
            self.LastUpdateTime = 0;
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

        public static void BuffFactory(this BuffManagerComponentC self, M2C_UnitBuffUpdate message)
        {
            InitBuffData initBuffData = new InitBuffData();
            initBuffData.TargetAngle = 0;
            initBuffData.BuffConfigId = message.BuffConfigId;
            initBuffData.Spellcaster = message.Spellcaster;
            initBuffData.BuffEndTime = message.BuffEndTime;
            initBuffData.UnitType = message.UnitType;
            initBuffData.UnitConfigId = message.UnitConfigId;
            initBuffData.SkillConfigId = message.SkillConfigId;
            initBuffData.UnitIdFrom = message.UnitIdFrom;
            initBuffData.TargetPostion = new float3(message.TargetPostion[0], message.TargetPostion[1], message.TargetPostion[2]);
            
            
            BuffC buff = self.AddChildWithId<BuffC>(message.BuffId, true);
            buff.OnInit(initBuffData, self.GetParent<Unit>());
            buff.OnExecute();
            self.Buffs.Add(buff);

            if (self.Timer == 0)
            {
                self.LastUpdateTime = TimeInfo.Instance.ClientNow();
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.BuffTimerC, self);
            }
        }

        public static void RemoveBuff(this BuffManagerComponentC self, long buffId)
        {
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                BuffC buff = self.Buffs[i];

                if (buff.Id == buffId)
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
                if (buff.InitBuffData.BuffConfigId == buffConfigId)
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
                if (buff.InitBuffData.BuffConfigId != buffConfigId)
                {
                    continue;
                }

                if (formId != 0 && formId != buff.InitBuffData.UnitIdFrom)
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