using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class HeroComponentC : Entity, IAwake, IDestroy
    {
        public Dictionary<long, EntityRef<Hero>> Heros = new();
    }
}