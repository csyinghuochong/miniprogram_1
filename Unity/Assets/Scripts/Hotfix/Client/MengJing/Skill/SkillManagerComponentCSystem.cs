using System;
using Unity.Mathematics;

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
                    Log.Error(e);
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

            self.PublicCD -= deltaTime;
            if (self.PublicCD < 0)
            {
                self.PublicCD = 0;
            }
        }

        public static async ETTask<int> TryUseSkill(this SkillManagerComponentC self, int skillConfigId, long targetId, float angle, float3 position)
        {
            if (!SkillConfigCategory.Instance.DataMap.ContainsKey(skillConfigId))
            {
                return ErrorCode.ERR_ModifyData;
            }

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillConfigId);

            int errorCode = self.IsCanUseSkill(skillConfigId);
            if (errorCode != ErrorCode.ERR_Success)
            {
                return errorCode;
            }

            if (string.IsNullOrEmpty(skillConfig.SkillHandler))
            {
                return ErrorCode.ERR_NotSkillHandler;
            }

            C2M_TryUseSkill request = C2M_TryUseSkill.Create();
            request.SkillConfigId = skillConfigId;
            request.TargetId = targetId;
            request.Angle = angle;
            request.Position = position;

            M2C_TryUseSkill response = (M2C_TryUseSkill)await self.Root().GetComponent<ClientSenderComponent>().Call(request);
            return response.Error;
        }

        public static void OnUseSkill(this SkillManagerComponentC self, M2C_OnUseSkill message)
        {
            self.AddSkillCD(message.SkillConfigId, message.CD, message.PublicCD);

            Unit unit = self.GetParent<Unit>();

            UseSkillInfo useSkillInfo = new UseSkillInfo();
            useSkillInfo.SkillConfigId = message.SkillConfigId;
            useSkillInfo.TargetId = message.TargetId;
            useSkillInfo.Angle = message.Angle;
            useSkillInfo.Position = message.Position;

            SkillC skill = self.AddChild<SkillC>();
            self.Skills.Add(skill);
            skill.OnInit(useSkillInfo, unit);
            skill.OnExecute();
        }

        public static int IsCanUseSkill(this SkillManagerComponentC self, int skillConfigId)
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

        private static void AddSkillCD(this SkillManagerComponentC self, int skillConfigId, float cd, float publicCD)
        {
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

            skillCDItem.CD = cd;

            self.PublicCD = publicCD;
        }
    }
}