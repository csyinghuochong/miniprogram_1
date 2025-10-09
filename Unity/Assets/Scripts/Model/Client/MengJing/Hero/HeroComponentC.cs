using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class HeroComponentC : Entity, IAwake, IDestroy
    {
        public Dictionary<long, EntityRef<Hero>> Heros = new();

        public int CurrentFormationIndex { get; set; }
        public List<long> Formation_1 { get; set; } = new();
        public List<long> Formation_2 { get; set; } = new();
    }
}