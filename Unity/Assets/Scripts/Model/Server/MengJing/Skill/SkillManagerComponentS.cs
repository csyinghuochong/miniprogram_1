using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class SkillManagerComponentS : Entity, IAwake, IDestroy
    {
        public long Timer;
        public int TimeInterval;
        public float PublicCD;
        public List<EntityRef<SkillS>> Skills = new();
        public List<SkillCDItem> SkillCDs { get; set; } = new();
    }
}