using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    public enum EffectState
    {
        None,
        WaitRemove,
        Running,
        Finished,
    }

    [ChildOf(typeof(EffectViewComponent))]
    public class Effect : Entity, IAwake, IDestroy
    {
        public EffectState EffectState { get; set; }
        public EffectConfig EffectConfig { get; set; }
        public EffectData EffectData { get; set; }

        private EntityRef<Unit> theUnitBelongTo;

        // 寄生于哪个Unit，并不代表当前Buff实际寄居者，需要通过GetBuffTarget来获取，因为它赋值于Buff链起源的地方，具体值取决于那个起源Buff
        public Unit TheUnitBelongTo { get => this.theUnitBelongTo; set => this.theUnitBelongTo = value; }

        public float ElapsedTime { get; set; }
        public float HideObjTime; //隐藏物体间隔时间    
        public GameObject EffectObj { get; set; }
        public string EffectPath;
        public float3 EffectPosition;
        public float EffectAngle;
    }
}