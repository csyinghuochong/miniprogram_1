using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class BuffManagerComponentS : Entity, IAwake, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public List<EntityRef<BuffS>> Buffs = new();
        public bool Checking { get; set; }
    }
}