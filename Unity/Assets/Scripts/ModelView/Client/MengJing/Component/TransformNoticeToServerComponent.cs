using Unity.Mathematics;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class TransformNoticeToServerComponent : Entity, IAwake, IDestroy
    {
        private EntityRef<Unit> myUnit;
        public Unit MyUnit { get => this.myUnit; set => this.myUnit = value; }
        
        public float3 Position;

        public long Timer;
    }
}