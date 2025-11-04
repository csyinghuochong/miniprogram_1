using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [ChildOf(typeof(SkillManagerComponentC))]
    public class SkillC : Entity, IAwake, IDestroy
    {
        public UseSkillInfo UseSkillInfo { get; set; }
        public SkillConfig SkillConfig { get; set; }
        public SkillHandlerC SkillHandler { get; set; }
        public SkillState SkillState { get; set; }
        public float SkillLiveTime { get; set; }
        private EntityRef<Unit> theUnitFrom; //来自哪个Unit
        public Unit TheUnitFrom { get => this.theUnitFrom; set => this.theUnitFrom = value; }
        private EntityRef<Unit> theUnitTarget;
        public Unit TheUnitTarget { get => this.theUnitTarget; set => this.theUnitTarget = value; }
        public float3 NowPosition { get; set; } //当前技能的坐标点
        public float3 TargetPosition { get; set; }
        public float ElapsedTime { get; set; }
        public float Speed { get; set; }
        public List<long> EffectInstanceId { get; set; } = new();
    }
}