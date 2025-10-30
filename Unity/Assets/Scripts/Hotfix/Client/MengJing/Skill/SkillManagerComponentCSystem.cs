using System;

namespace ET.Client
{
    [EntitySystemOf(typeof(SkillManagerComponentC))]
    [FriendOf(typeof(SkillManagerComponentC))]
    public static partial class SkillManagerComponentCSystem
    {
        [Invoke(TimerInvokeType.SkillTimerC)]
        public class SkillTimerC : ATimer<SkillManagerComponentC>
        {
            protected override void Run(SkillManagerComponentC self)
            {
                try
                {
                    self.Update();
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }
            }
        }

        [EntitySystem]
        private static void Awake(this SkillManagerComponentC self)
        {
            self.TimeInterval = 33;
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(self.TimeInterval, TimerInvokeType.SkillTimerC, self);
        }

        [EntitySystem]
        private static void Destroy(this SkillManagerComponentC self)
        {
            self.Skills.Clear();
            self.Skills = null;
            self.SkillCDs.Clear();
            self.SkillCDs = null;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Update(this SkillManagerComponentC self)
        {
            float deltaTime = self.TimeInterval / 1000f * self.Scene().TimeScale;

            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillC skill = self.Skills[i];

                if (skill.SkillState == SkillState.Finished)
                {
                    skill.Dispose();
                    self.Skills.RemoveAt(i);
                    continue;
                }

                skill.OnUpdate(deltaTime);
            }

            foreach (SkillCDItem skillCdItem in self.SkillCDs)
            {
                skillCdItem.CD -= deltaTime;
                if (skillCdItem.CD < 0)
                {
                    skillCdItem.CD = 0;
                }
            }
        }

        public static int OnUseSkill(this SkillManagerComponentC self, SkillInfo skillInfo)
        {
            if (!SkillConfigCategory.Instance.DataMap.ContainsKey(skillInfo.SkillConfigId))
            {
                return ErrorCode.ERR_ModifyData;
            }

            Unit unit = self.GetParent<Unit>();

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillInfo.SkillConfigId);

            int errorCode = self.IsCanUseSkill(skillInfo.SkillConfigId);
            if (errorCode != ErrorCode.ERR_Success)
            {
                return errorCode;
            }

            if (string.IsNullOrEmpty(skillConfig.SkillHandler))
            {
                return ErrorCode.ERR_ModifyData;
            }

            self.AddSkillCD(skillInfo.SkillConfigId);

            SkillC skill = self.AddChild<SkillC>();
            self.Skills.Add(skill);
            skill.OnInit(skillInfo, unit);
            skill.OnExecute();

            return ErrorCode.ERR_Success;
        }

        public static int IsCanUseSkill(this SkillManagerComponentC self, int nowSkillID)
        {
            SkillCDItem skillCdItem = null;

            foreach (SkillCDItem skillCDItem in self.SkillCDs)
            {
                if (skillCDItem.SkillConfigId == nowSkillID)
                {
                    skillCdItem = skillCDItem;
                    break;
                }
            }

            if (skillCdItem == null)
            {
                return ErrorCode.ERR_Success;
            }

            if (skillCdItem.CD > 0)
            {
                return ErrorCode.ERR_UseSkillInCD;
            }

            return ErrorCode.ERR_Success;
        }

        private static void AddSkillCD(this SkillManagerComponentC self, int skillConfigId)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillConfigId);

            SkillCDItem skillCDItem = null;
            foreach (SkillCDItem skillCdItem in self.SkillCDs)
            {
                if (skillCdItem.SkillConfigId == skillConfigId)
                {
                    skillCDItem = skillCdItem;
                    break;
                }
            }

            if (skillCDItem == null)
            {
                skillCDItem = new SkillCDItem();
                skillCDItem.SkillConfigId = skillConfigId;
                self.SkillCDs.Add(skillCDItem);
            }

            if (skillConfig.SkillActType == (int)SkillActType.Normal)
            {
                // 普通攻击
                skillCDItem.CD = 1f;
            }
            else
            {
                skillCDItem.CD = (float)skillConfig.SkillCD;
            }
        }
    }
}