using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BattlePassComponentC : Entity, IAwake, IDestroy
    {
        public List<EntityRef<BattlePass>> BattlePassList = new();
    }
}