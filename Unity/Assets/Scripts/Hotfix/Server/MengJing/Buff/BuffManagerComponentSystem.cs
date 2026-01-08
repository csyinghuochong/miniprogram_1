using System;
using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(BuffManagerComponent))]
    [FriendOf(typeof(BuffManagerComponent))]
    public static partial class BuffManagerComponentSystem
    {
        [Invoke(TimerInvokeType.BuffTimerS)]
        public class BuffTimerS : ATimer<BuffManagerComponent>
        {
            protected override void Run(BuffManagerComponent self)
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
        private static void Awake(this BuffManagerComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BuffManagerComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);
            self.Buffs.Clear();
        }

        private static void Update(this BuffManagerComponent self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                Buff buff = self.Buffs[i];

                if (buff.BuffState == BuffState.Finished)
                {
                    self.OnRemoveBuffItem(buff);
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

        public static void BuffFactory(this BuffManagerComponent self, InitBuffData initBuffData, Unit from, Skill skill, bool notice = true)
        {
            Unit unit = self.GetParent<Unit>();
            BuffConfig newBuffConfig = BuffConfigCategory.Instance.Get(initBuffData.BuffConfigId);

            // 叠加
            if (newBuffConfig.BuffMaxStackCount != 0)
            {
                int curNumber = 0;
                for (int i = self.Buffs.Count - 1; i >= 0; i--)
                {
                    Buff buff = self.Buffs[i];

                    BuffConfig oldBuffConfig = buff.BuffConfig;
                    if (oldBuffConfig.Id == newBuffConfig.Id)
                    {
                        curNumber++;
                    }
                }

                if (curNumber >= newBuffConfig.BuffMaxStackCount)
                {
                    return;
                }
            }

            // 移除
            bool addSameState = false; // 添加相同角色状态
            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                bool remove = false;
                Buff buff = self.Buffs[i];
                BuffConfig oldBuffConfig = buff.BuffConfig;

                if (oldBuffConfig.Id == newBuffConfig.Id && newBuffConfig.IsBuffStackable == 0)
                {
                    remove = true;
                }

                if (oldBuffConfig.BuffType == 2 && oldBuffConfig.BuffType == newBuffConfig.BuffType &&
                    oldBuffConfig.BuffParameterType == newBuffConfig.BuffParameterType)
                {
                    if (buff.BuffEndTime > 0)
                    {
                        addSameState = true;
                    }
                    else
                    {
                        remove = true;
                    }
                }

                if (remove)
                {
                    buff.BuffState = BuffState.Finished;
                }
            }

            if (addSameState)
            {
                return;
            }

            Buff newBuff = self.AddChild<Buff>();
            newBuff.OnInit(initBuffData, from, unit, skill);

            self.Buffs.Insert(0, newBuff);

            if (self.Timer == 0)
            {
                self.LastUpdateTime = TimeInfo.Instance.ClientNow();
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.BuffTimerS, self);
            }

            if (notice)
            {
                M2C_UnitBuffUpdate m2C_UnitBuffUpdate = M2C_UnitBuffUpdate.Create(true);
                m2C_UnitBuffUpdate.UnitId = unit.Id;
                m2C_UnitBuffUpdate.BuffId = newBuff.Id;
                m2C_UnitBuffUpdate.BuffConfigId = newBuffConfig.Id;
                m2C_UnitBuffUpdate.BuffOperateType = 1;
                m2C_UnitBuffUpdate.BuffEndTime = newBuff.BuffEndTime;
                m2C_UnitBuffUpdate.TargetPostion.Clear();
                m2C_UnitBuffUpdate.TargetPostion.Add(newBuff.TargetPosition.x);
                m2C_UnitBuffUpdate.TargetPostion.Add(newBuff.TargetPosition.y);
                m2C_UnitBuffUpdate.TargetPostion.Add(newBuff.TargetPosition.z);
                m2C_UnitBuffUpdate.Spellcaster = from.GetComponent<UnitInfoComponent>().UnitName;
                m2C_UnitBuffUpdate.UnitType = from.Type;
                m2C_UnitBuffUpdate.UnitConfigId = from.ConfigId;
                m2C_UnitBuffUpdate.SkillConfigId = initBuffData.SkillConfigId;
                m2C_UnitBuffUpdate.UnitIdFrom = from.Id;

                MapMessageHelper.Broadcast(unit, m2C_UnitBuffUpdate);
            }
        }

        private static void OnRemoveBuffItem(this BuffManagerComponent self, Buff buff)
        {
            M2C_UnitBuffRemove m2C_UnitBuffUpdate = M2C_UnitBuffRemove.Create(true);
            m2C_UnitBuffUpdate.UnitId = self.GetParent<Unit>().Id;
            m2C_UnitBuffUpdate.BuffId = buff.Id;
            MapMessageHelper.Broadcast(self.GetParent<Unit>(), m2C_UnitBuffUpdate);
        }

        public static void OnFinish(this BuffManagerComponent self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);

            for (int i = self.Buffs.Count - 1; i >= 0; i--)
            {
                Buff buff = self.Buffs[i];
                self.OnRemoveBuffItem(buff);
                buff.OnFinished();
                buff.Dispose();
                self.Buffs.RemoveAt(i);
            }
        }
    }
}