using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class Move2DComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public int TimeInterval;
        public float Speed { get; set; }
        public List<float3> Targets { get; set; } = new();
    }
}