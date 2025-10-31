using System;
using Unity.Mathematics;

namespace ET.Server
{
    [EntitySystemOf(typeof(SkillManagerComponentS))]
    [FriendOf(typeof(SkillManagerComponentS))]
    public static partial class SkillManagerComponentSSystem
    {
        [Invoke(TimerInvokeType.SkillTimerS)]
        public class SkillTimerS : ATimer<SkillManagerComponentS>
        {
            protected override void Run(SkillManagerComponentS self)
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
        private static void Awake(this SkillManagerComponentS self)
        {
            self.TimeInterval = 100;
            self.Timer = self.Root().GetComponent<TimerComponent>().NewRepeatedTimer(self.TimeInterval, TimerInvokeType.SkillTimerS, self);
        }

        [EntitySystem]
        private static void Destroy(this SkillManagerComponentS self)
        {
            self.Skills.Clear();
            self.Skills = null;
            self.SkillCDs.Clear();
            self.SkillCDs = null;
            self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
        }

        private static void Update(this SkillManagerComponentS self)
        {
            float deltaTime = self.TimeInterval / 1000f * self.Scene().TimeScale;

            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillS skill = self.Skills[i];

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

            self.PublicCD -= deltaTime;
            if (self.PublicCD < 0)
            {
                self.PublicCD = 0;
            }
        }

        public static int TryUseSkill(this SkillManagerComponentS self, int skillConfigId, long targetId, float angle, float3 position)
        {
            if (!SkillConfigCategory.Instance.DataMap.ContainsKey(skillConfigId))
            {
                return ErrorCode.ERR_ModifyData;
            }

            Unit unit = self.GetParent<Unit>();

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillConfigId);

            int errorCode = self.IsCanUseSkill(skillConfigId);
            if (errorCode != ErrorCode.ERR_Success)
            {
                return errorCode;
            }

            if (string.IsNullOrEmpty(skillConfig.SkillHandler))
            {
                return ErrorCode.ERR_ModifyData;
            }

            float cd = self.AddSkillCD(skillConfigId);

            UseSkillInfo useSkillInfo = new UseSkillInfo();
            useSkillInfo.SkillConfigId = skillConfigId;
            useSkillInfo.TargetId = targetId;
            useSkillInfo.Angle = angle;
            useSkillInfo.Position = position;

            SkillS skill = self.AddChild<SkillS>();
            self.Skills.Add(skill);
            skill.OnInit(useSkillInfo, unit);
            skill.OnExecute();

            M2C_OnUseSkill message = M2C_OnUseSkill.Create();
            message.UnitId = unit.Id;
            message.SkillConfigId = skillConfigId;
            message.TargetId = targetId;
            message.Angle = angle;
            message.Position = position;
            message.CD = cd;
            message.PublicCD = self.PublicCD;

            MapMessageHelper.Broadcast(self.GetParent<Unit>(), message);

            return ErrorCode.ERR_Success;
        }

        public static int IsCanUseSkill(this SkillManagerComponentS self, int skillConfigId)
        {
            SkillCDItem skillCdItem = null;

            foreach (SkillCDItem skillCDItem in self.SkillCDs)
            {
                if (skillCDItem.SkillConfigId == skillConfigId)
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

            if (self.PublicCD > 0)
            {
                return ErrorCode.ERR_UseSkillInPublicCD;
            }

            return ErrorCode.ERR_Success;
        }

        private static float AddSkillCD(this SkillManagerComponentS self, int skillConfigId)
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
                skillCDItem.CD = 1 / self.GetParent<Unit>().GetComponent<NumericComponentS>().GetAsFloat(NumericType.Now_AtkSpeed);
                self.PublicCD = 0f;
            }
            else
            {
                skillCDItem.CD = (float)skillConfig.SkillCD;
                self.PublicCD = 0.5f;
            }

            return skillCDItem.CD;
        }
    }
}