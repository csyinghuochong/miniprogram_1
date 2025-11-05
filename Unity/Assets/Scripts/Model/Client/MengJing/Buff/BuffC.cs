using Unity.Mathematics;

namespace ET.Client
{
    [ChildOf(typeof(BuffManagerComponentC))]
    public class BuffC : Entity, IAwake, IDestroy
    {
        public BuffData buffData { get; set; }
        public BuffState BuffState { get; set; }
        public BuffHandler BuffHandler { get; set; }
        public BuffConfig BuffConfig { get; set; }
    }
}