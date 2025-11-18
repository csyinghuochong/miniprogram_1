using System.Collections.Generic;

namespace ET.Server
{
    [EnableClass]
    public class SkillPassiveInfo
    {
        public int SkillId;
        public List<PassiveSkillType> SkillPassiveTypeEnum;
        public List<float> SkillPro;
        public int TriggerOnce;
        public long TriggerInterval;
        public long LastTriggerTime;
        public int TriggerNumber;

        public SkillPassiveInfo(int skillId, List<PassiveSkillType> skillPassiveTypeEnum, List<float> skillPro, int triggerOnce, double triggerTime)
        {
            this.SkillId = skillId;
            this.SkillPassiveTypeEnum = skillPassiveTypeEnum;
            this.SkillPro = skillPro;
            this.TriggerOnce = triggerOnce;
            this.TriggerInterval = (long)(1000 * triggerTime);
            this.LastTriggerTime = 0;
        }

        public void Reset()
        {
            this.LastTriggerTime = 0;
        }
    }

    [ComponentOf(typeof(Unit))]
    public class SkillPassiveComponent : Entity, IAwake, IDestroy, ITransfer
    {
        public long Timer;
        public long LastUpdateTime;

        public List<SkillPassiveInfo> SkillPassiveInfos = new();
    }
}