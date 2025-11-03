using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class Move2DComponent : Entity, IAwake, IDestroy
    {
        private EntityRef<Unit> unit;
        public Unit Unit { get => unit; set => unit = value; }

        public long Timer;
        public int TimeInterval;
        public float Speed { get; set; }
        public List<float3> Targets { get; set; } = new();
    }
}