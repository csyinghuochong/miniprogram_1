using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class BuffManagerComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public List<EntityRef<Buff>> Buffs = new();
    }
}