using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class BuffManagerComponentC : Entity, IAwake, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public List<EntityRef<BuffC>> Buffs = new();
    }
}