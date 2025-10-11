using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(SkillManagerComponent))]
    [FriendOf(typeof(SkillManagerComponent))]
    public static partial class SkillManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SkillManagerComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this SkillManagerComponent self)
        {
            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                Skill skill = self.Skills[i];
                skill.SkillLiveTime -= Time.deltaTime;
                skill.SkillHandler.OnUpdate(skill);

                if (skill.SkillState == SkillState.Finished)
                {
                    skill.SkillHandler.OnFinished(skill);
                    skill.Dispose();
                    self.Skills.RemoveAt(i);
                }
            }

            foreach (SkillCDItem skillCdItem in self.SkillCDs)
            {
                skillCdItem.CD -= Time.deltaTime;
                if (skillCdItem.CD < 0)
                {
                    skillCdItem.CD = 0;
                }
            }
        }

        [EntitySystem]
        private static void Destroy(this SkillManagerComponent self)
        {
            self.Skills.Clear();
            self.Skills = null;
            self.SkillCDs.Clear();
            self.SkillCDs = null;
        }

        public static int OnUseSkill(this SkillManagerComponent self, SkillInfo skillInfo)
        {
            if (!SkillConfigCategory.Instance.Contain(skillInfo.SkillConfigId))
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

            SkillHandler skillHandler = SkillDispatcherComponent.Instance.Get(skillConfig.SkillHandler);
            Skill skill = self.AddChild<Skill>();
            skill.SkillInfo = skillInfo;
            skill.SkillConfig = skillConfig;
            skill.SkillHandler = skillHandler;
            self.Skills.Add(skill);

            self.AddSkillCD(skillInfo.SkillConfigId);

            skillHandler.OnInit(skill, unit);
            skillHandler.OnExecute(skill);

            return ErrorCode.ERR_Success;
        }

        public static int IsCanUseSkill(this SkillManagerComponent self, int nowSkillID)
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
                return ErrorCode.ERR_UseSkillInCD3;
            }

            return ErrorCode.ERR_Success;
        }

        private static void AddSkillCD(this SkillManagerComponent self, int skillConfigId)
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