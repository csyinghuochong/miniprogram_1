namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class LocalLevelComponent : Entity, IAwake, IDestroy
    {
        private EntityRef<Unit> mainUnit;
        public Unit MainUnit { get => this.mainUnit; set => this.mainUnit = value; }

        public int CurrentLevelId;
        public int CurrentWaveIndex;
        public int CurrentWaveId;
    }
}