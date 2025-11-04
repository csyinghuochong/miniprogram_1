using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class EffectViewComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        public List<Effect> Effects { get; set; } = new();
    }
}