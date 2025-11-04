using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class SkillManagerComponentC : Entity, IAwake, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public float PublicCD;
        public List<EntityRef<SkillC>> Skills = new();
        public List<SkillCDItem> SkillCDs { get; set; } = new();
    }
}