using Unity.Mathematics;

namespace ET.Server
{
    [ChildOf(typeof(BuffManagerComponentS))]
    public class BuffS : Entity, IAwake, IDestroy
    {
        public float BuffEndTime { get; set; }
        public BuffData BuffData { get; set; }
        public BuffState BuffState { get; set; }
        public BuffSHandler BuffHandler { get; set; }
        public BuffConfig BuffConfig { get; set; }

        /// <summary>
        /// 来自哪个Unit
        /// </summary>
        public Unit TheUnitFrom { get; set; }

        /// <summary>
        /// 寄生于哪个Unit，并不代表当前Buff实际寄居者，需要通过GetBuffTarget来获取，因为它赋值于Buff链起源的地方，具体值取决于那个起源Buff
        /// </summary>
        public Unit TheUnitBelongTo { get; set; }

        public float3 TargetPosition { get; set; }
    }
}