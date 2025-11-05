using Unity.Mathematics;

namespace ET.Client
{
    [ChildOf(typeof(BuffManagerComponentC))]
    public class BuffC : Entity, IAwake, IDestroy
    {
        public BuffData BuffData { get; set; }
        public BuffState BuffState { get; set; }
        public BuffCHandler BuffHandler { get; set; }
        public BuffConfig BuffConfig { get; set; }

        private EntityRef<Unit> theUnitBelongTo;
        public Unit TheUnitBelongTo { get => this.theUnitBelongTo; set => this.theUnitBelongTo = value; }
        public float BuffEndTime { get; set; }
        public float RunTime { get; set; }

        public EffectData EffectData { get; set; }
        public long EffectInstanceId { get; set; }
    }
}