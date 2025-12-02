using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class FloatingTextComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public List<EntityRef<FloatingText>> FloatingTexts = new();
    }
}