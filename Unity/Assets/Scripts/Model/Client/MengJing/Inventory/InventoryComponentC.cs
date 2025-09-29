using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class InventoryComponentC : Entity, IAwake, IDestroy
    {
        public Dictionary<long, EntityRef<Item>> Items = new();
    }
}