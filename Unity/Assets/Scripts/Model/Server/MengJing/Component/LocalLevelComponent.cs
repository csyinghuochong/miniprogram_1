using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class LocalLevelComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public long LastUpdateTime;

        private EntityRef<Unit> mainUnit;
        public Unit MainUnit { get => this.mainUnit; set => this.mainUnit = value; }

        public bool WaitPlayerEnterBossRoom;
        public int SpawnedMonsterIndex;
        public float SpawnTime;

        public float3 LastPlayerPosition;
    }
}