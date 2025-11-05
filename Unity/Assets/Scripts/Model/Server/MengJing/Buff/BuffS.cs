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
        public float3 TargetPosition { get; set; }
    }
}