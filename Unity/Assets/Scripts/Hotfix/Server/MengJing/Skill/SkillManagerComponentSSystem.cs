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
            long now = TimeInfo.Instance.ClientNow();
            float deltaTime = (now - self.LastUpdateTime) / 1000f * self.Scene().TimeScale;
            self.LastUpdateTime = now;

            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillS skill = self.Skills[i];

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

        public static int TryUseSkill(this SkillManagerComponentS self, int skillConfigId, long targetId, float angle, float3 position)
        {
            if (!SkillConfigCategory.Instance.DataMap.ContainsKey(skillConfigId))
            {
                return ErrorCode.ERR_ModifyData;
            }

            Unit myUnit = self.GetParent<Unit>();
            UnitComponent unitComponent = myUnit.GetParent<UnitComponent>();

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillConfigId);

            if (string.IsNullOrEmpty(skillConfig.SkillHandler) || SkillDispatcherComponentS.Instance.Get(skillConfig.SkillHandler) == null)
            {
                return ErrorCode.ERR_ModifyData;
            }

            int errorCode = self.IsCanUseSkill(skillConfigId);
            if (errorCode != ErrorCode.ERR_Success)
            {
                return errorCode;
            }

            InitSkillData initSkillData = new InitSkillData();
            initSkillData.SkillConfigId = skillConfigId;
            initSkillData.TargetId = targetId;
            initSkillData.Angle = angle;
            initSkillData.TargetPosition = position;

            Unit targetUnit = myUnit.GetParent<UnitComponent>().Get(targetId);

            switch (skillConfig.SkillTargetType)
            {
                case SkillTargetType.SelfPosition:
                {
                    initSkillData.TargetId = myUnit.Id;
                    initSkillData.TargetPosition = myUnit.Position;
                    break;
                }
                case SkillTargetType.TargetPositon:
                {
                    if (targetUnit == null)
                    {
                        return ErrorCode.ERR_TargetUnitIsNull;
                    }

                    if (!myUnit.IsCanAttackUnit(targetUnit))
                    {
                        return ErrorCode.ERR_TargetUnitCantBeAttack;
                    }

                    initSkillData.TargetId = targetUnit.Id;
                    initSkillData.TargetPosition = targetUnit.Position;

                    break;
                }
                case SkillTargetType.TargetOnly:
                {
                    if (targetUnit == null)
                    {
                        return ErrorCode.ERR_TargetUnitIsNull;
                    }

                    if (!myUnit.IsCanAttackUnit(targetUnit))
                    {
                        return ErrorCode.ERR_TargetUnitCantBeAttack;
                    }

                    initSkillData.TargetId = targetUnit.Id;
                    initSkillData.TargetPosition = targetUnit.Position;

                    break;
                }
                case SkillTargetType.SelfOnly:
                {
                    initSkillData.TargetId = myUnit.Id;
                    initSkillData.TargetPosition = myUnit.Position;

                    break;
                }
                case SkillTargetType.MulTarget:
                case SkillTargetType.AllTeam:
                case SkillTargetType.AllEnemy:
                {
                    initSkillData.TargetPosition = myUnit.Position;
                    break;
                }
            }

            float cd = self.AddSkillCD(skillConfigId);

            SkillS skill = self.AddChild<SkillS>();
            self.Skills.Add(skill);
            skill.OnInit(initSkillData, myUnit);
            skill.OnExecute();

            if (self.Timer == 0)
            {
                self.LastUpdateTime = TimeInfo.Instance.ClientNow();
                self.Timer = self.Root().GetComponent<TimerComponent>().NewFrameTimer(TimerInvokeType.SkillTimerS, self);
            }

            M2C_OnUseSkill message = M2C_OnUseSkill.Create(true);
            message.UnitId = myUnit.Id;
            message.SkillId = skill.Id;
            message.SkillConfigId = initSkillData.SkillConfigId;
            message.TargetId = initSkillData.TargetId;
            message.Angle = initSkillData.Angle;
            message.Position = initSkillData.TargetPosition;
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

            int error = self.GetParent<Unit>().IsCanMove();
            if (error != ErrorCode.ERR_Success)
            {
                return error;
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

        public static void OnFinish(this SkillManagerComponentS self, bool notice)
        {
            self.Root().GetComponent<TimerComponent>()?.Remove(ref self.Timer);

            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillS skill = self.Skills[i];
                skill.OnFinished();
                skill.Dispose();
                self.Skills.RemoveAt(i);
            }

            Unit unit = self.GetParent<Unit>();
            if (notice && unit != null && !unit.IsDisposed)
            {
                M2C_UnitFinishSkill message = M2C_UnitFinishSkill.Create(true);
                message.UnitId = unit.Id;
                MapMessageHelper.Broadcast(unit, message);
            }
        }

        public static void OnBreak(this SkillManagerComponentS self)
        {
            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillS skill = self.Skills[i];
                skill.OnFinished();
                skill.Dispose();
                self.Skills.RemoveAt(i);
            }
        }

        private static void OnRemoveSkillItem(this SkillManagerComponentS self, SkillS skill)
        {
            M2C_UnitSkillRemove message = M2C_UnitSkillRemove.Create(true);
            message.UnitId = self.GetParent<Unit>().Id;
            message.SkillId = skill.Id;
            MapMessageHelper.Broadcast(self.GetParent<Unit>(), message);
        }
    }
}