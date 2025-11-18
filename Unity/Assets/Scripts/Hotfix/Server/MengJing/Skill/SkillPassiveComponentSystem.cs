using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(SkillPassiveComponent))]
    [FriendOf(typeof(SkillPassiveComponent))]
    public static partial class SkillPassiveComponentSystem
    {
        [Invoke(TimerInvokeType.SkillPassive)]
        public class SkillPassiveTimer : ATimer<SkillPassiveComponent>
        {
            protected override void Run(SkillPassiveComponent self)
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
        private static void Awake(this SkillPassiveComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this SkillPassiveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Update(this SkillPassiveComponent self)
        {
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            Unit unit = self.GetParent<Unit>();

            self.OnTriggerPassiveSkill(SkillPassiveType.Type_3, unit.Id);
        }

        public static void AddPassiveSkill(this SkillPassiveComponent self, int skillConfigId)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillConfigId);

            if (skillConfig.SkillType != SkillType.Passive || SkillHelper.havePassiveSkillType(skillConfig.SkillPassiveType, SkillPassiveType.None))
            {
                return;
            }

            for (int i = 0; i < self.SkillPassiveInfos.Count; i++)
            {
                if (self.SkillPassiveInfos[i].SkillConfigId == skillConfig.Id)
                {
                    return;
                }
            }

            List<SkillPassiveType> passiveSkillType = new();
            List<float> passiveSkillPro = new();
            for (int i = 0; i < skillConfig.SkillPassiveType.Length; i++)
            {
                passiveSkillType.Add(skillConfig.SkillPassiveType[i]);
                passiveSkillPro.Add(skillConfig.PassiveSkillPro[i]);
            }

            SkillPassiveInfo skillPassiveInfo = new SkillPassiveInfo(skillConfig.Id, passiveSkillType, passiveSkillPro, skillConfig.PassiveSkillTriggerOnce, skillConfig.SkillCD);
            self.SkillPassiveInfos.Add(skillPassiveInfo);

            if (self.Timer == 0)
            {
                self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(1000, TimerInvokeType.SkillPassive, self);
            }
        }

        public static void RemovePassiveSkill(this SkillPassiveComponent self, int skillConfigId)
        {
            for (int i = self.SkillPassiveInfos.Count - 1; i >= 0; i--)
            {
                if (self.SkillPassiveInfos[i].SkillConfigId != skillConfigId)
                {
                    continue;
                }

                self.SkillPassiveInfos.RemoveAt(i);
                break;
            }
        }

        public static void OnTriggerPassiveSkill(this SkillPassiveComponent self, SkillPassiveType skillPassiveType, long targetId = 0, int skillId = 0)
        {
            Unit unit = self.GetParent<Unit>();

            List<SkillPassiveInfo> skillPassiveInfos = new();

            foreach (SkillPassiveInfo skillPassiveInfo in self.SkillPassiveInfos)
            {
                if (!skillPassiveInfo.SkillPassiveTypes.Contains(skillPassiveType))
                {
                    continue;
                }

                skillPassiveInfos.Add(skillPassiveInfo);
            }

            if (skillPassiveInfos.Count == 0)
            {
                return;
            }

            foreach (SkillPassiveInfo skillPassiveInfo in skillPassiveInfos)
            {
                float skillProValue = skillPassiveInfo.SkillPro[skillPassiveInfo.SkillPassiveTypes.IndexOf(skillPassiveType)];

                // 只触发一次
                if (skillPassiveInfo.LastTriggerTime > 0 && skillPassiveInfo.TriggerOnce == 1)
                {
                    continue;
                }

                // 触发限制
                if (skillPassiveInfo.TriggerInterval > 0 && skillPassiveInfo.RunTime - skillPassiveInfo.LastTriggerTime < skillPassiveInfo.TriggerInterval)
                {
                    continue;
                }

                bool trigger = false;
                switch (skillPassiveType)
                {
                    case SkillPassiveType.Type_1:
                    case SkillPassiveType.Type_2:
                    {
                        trigger = skillProValue >= RandomHelper.RandFloat01();
                        break;
                    }
                    case SkillPassiveType.Type_3:
                    {
                        NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();

                        long nowHp = numericComponent.GetAsLong(NumericType.Now_Hp);
                        long maxHp = numericComponent.GetAsLong(NumericType.Now_MaxHp);
                        float hpPro = nowHp * 1f / maxHp;
                        trigger = hpPro <= skillProValue;

                        break;
                    }
                    case SkillPassiveType.Type_4:
                    {
                        trigger = true;
                        
                        break;
                    }
                    case SkillPassiveType.Type_5:
                    {
                        trigger = true;
                        
                        break;
                    }
                }

                if (!trigger)
                {
                    continue;
                }
                
                SkillManagerComponentS skillManagerComponent = unit.GetComponent<SkillManagerComponentS>();
                if (skillManagerComponent.IsCanUseSkill(skillPassiveInfo.SkillConfigId) != ErrorCode.ERR_Success)
                {
                    continue;
                }

                self.ImmediateUseSkill(skillPassiveInfo, targetId);

                skillPassiveInfo.LastTriggerTime = 0;
            }
        }

        private static void ImmediateUseSkill(this SkillPassiveComponent self, SkillPassiveInfo skillPassiveInfo, long targetId = 0)
        {
            Unit unit = self.GetParent<Unit>();
            List<long> targetIdList = new List<long>();
            AIComponent aiComponent = unit.GetComponent<AIComponent>();
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillPassiveInfo.SkillConfigId);

            if (aiComponent != null)
            {
                targetId = aiComponent.TargetId;
                Unit u = unit.GetParent<UnitComponent>().Get(targetId);
                if (u != null && skillConfig.SkillTargetType == SkillTargetType.TargetOnly && math.distance(unit.Position, u.Position) > aiComponent.ActDistance)
                {
                    return;
                }
                
                targetIdList.Add(targetId);
            }

            if (targetIdList.Count == 0)
            {
                targetId = targetId > 0 ? targetId : unit.Id;
                targetIdList.Add(targetId);
            }
            
            Unit target = unit.GetParent<UnitComponent>().Get(targetId);
            float angle = 0;
            if (target != null)
            {
                float3 direction = target.Position - unit.Position;
                angle = math.degrees(math.atan2(direction.x, direction.y));
            }
            
            SkillManagerComponentS skillManagerComponent = unit.GetComponent<SkillManagerComponentS>();
            foreach (long id in targetIdList)
            {
                skillManagerComponent.TryUseSkill(skillPassiveInfo.SkillConfigId, id, angle, float3.zero);
            }
        }

        public static void Stop(this SkillPassiveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }
    }
}