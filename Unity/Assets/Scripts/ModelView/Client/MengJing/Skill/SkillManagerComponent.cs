using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class SkillManagerComponent : Entity, IAwake, IFixedUpdate, IDestroy
    {
        public List<EntityRef<Skill>> Skills = new();
        public List<SkillCDItem> SkillCDs { get; set; } = new();
    }
}