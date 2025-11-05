using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class BuffManagerComponentC : Entity, IAwake, IUpdate, IDestroy
    {
        public List<EntityRef<BuffC>> Buffs = new();
    }
}