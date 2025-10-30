using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class SkillManagerComponentS : Entity, IAwake, IUpdate, IDestroy
    {
        public List<EntityRef<SkillS>> Skills = new();
        public List<SkillCDItem> SkillCDs { get; set; } = new();
    }
}