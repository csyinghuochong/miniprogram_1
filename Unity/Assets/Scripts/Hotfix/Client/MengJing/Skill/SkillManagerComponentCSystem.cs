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
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillC skill = self.Skills[i];

                if (skill.SkillState == SkillState.Finished)
                {
                    skill.OnFinished();
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

            bool hasActiveSkills = self.Skills.Count > 0;
            bool hasActiveCDs = self.PublicCD > 0 || self.SkillCDs.Exists(item => item.CD > 0);

            if (!hasActiveSkills && !hasActiveCDs)
            {
                self.Root().GetComponent<TimerComponent>().Remove(ref self.Timer);
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
            useSkillInfo.TargetPosition = message.Position;

            SkillC skill = self.AddChild<SkillC>();
            self.Skills.Add(skill);
            skill.OnInit(useSkillInfo, unit);
            skill.OnExecute();

            if (self.Timer == 0)
            {
                self.LastUpdateTime = TimeInfo.Instance.ClientNow();
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.SkillTimerC, self);
            }

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(message.SkillConfigId);

            if (!string.IsNullOrEmpty(skillConfig.SkillAnimation))
            {
                EventSystem.Instance.Publish(self.Root(), new FsmChange()
                {
                    FsmHandlerType = 4,
                    SkillId = message.SkillConfigId,
                    Unit = unit
                });
            }
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

        public static void OnFinish(this SkillManagerComponentC self)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);

            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillC skill = self.Skills[i];
                skill.OnFinished();
                skill.Dispose();
                self.Skills.RemoveAt(i);
            }
        }
    }
}