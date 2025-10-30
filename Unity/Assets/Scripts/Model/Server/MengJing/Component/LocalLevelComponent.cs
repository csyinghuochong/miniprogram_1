using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class LocalLevelComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public int TimeInterval;

        private EntityRef<Unit> mainUnit;
        public Unit MainUnit { get => this.mainUnit; set => this.mainUnit = value; }

        public bool WaitPlayerEnterBossRoom;
        public List<int> SpawnedMonsterBatchIds = new();
        public float SpawnTime;
    }
}