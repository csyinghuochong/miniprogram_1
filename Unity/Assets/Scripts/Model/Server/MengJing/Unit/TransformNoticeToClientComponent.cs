using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class TransformNoticeToClientComponent : Entity, IAwake, IDestroy
    {
        private EntityRef<Unit> myUnit;
        public Unit MyUnit { get => this.myUnit; set => this.myUnit = value; }

        private EntityRef<AOIEntity> aOIEntity;
        public AOIEntity AOIEntity { get => this.aOIEntity; set => this.aOIEntity = value; }

        public Dictionary<long, float3> UnitPositions = new();

        public long Timer;
    }
}