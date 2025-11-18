using System;
using System.Collections.Generic;

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
                passiveSkillPro.Add((float)skillConfig.PassiveSkillPro[i]);
            }

            SkillPassiveInfo skillPassiveInfo = new SkillPassiveInfo(skillConfig.Id, passiveSkillType, passiveSkillPro, skillConfig.PassiveSkillTriggerOnce, skillConfig.SkillCD);
            self.SkillPassiveInfos.Add(skillPassiveInfo);
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

        public static void OnTriggerPassiveSkill(this SkillPassiveComponent self, SkillPassiveType skillPassiveType)
        {
            
        }
        
        public static void Stop(this SkillPassiveComponent self)
        {
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }
    }
}