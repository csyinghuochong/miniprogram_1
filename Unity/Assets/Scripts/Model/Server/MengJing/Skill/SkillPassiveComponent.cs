using System.Collections.Generic;

namespace ET.Server
{
    [EnableClass]
    public class SkillPassiveInfo
    {
        public int SkillConfigId;
        public List<SkillPassiveType> SkillPassiveTypes;
        public List<float> SkillPro;
        public int TriggerOnce;
        public float TriggerInterval;
        public float LastTriggerTime;
        public float RunTime;
        public int TriggerNumber;

        public SkillPassiveInfo(int skillConfigId, List<SkillPassiveType> skillPassiveTypes, List<float> skillPro, int triggerOnce, float triggerTime)
        {
            this.SkillConfigId = skillConfigId;
            this.SkillPassiveTypes = skillPassiveTypes;
            this.SkillPro = skillPro;
            this.TriggerOnce = triggerOnce;
            this.TriggerInterval = triggerTime;
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