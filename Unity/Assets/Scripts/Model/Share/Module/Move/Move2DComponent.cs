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
        public long LastUpdateTime; // 上次更新的时间戳

        public float Speed { get; set; }
        public List<float3> Targets { get; set; } = new();
    }
}