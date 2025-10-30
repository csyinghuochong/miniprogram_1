using Unity.Mathematics;

namespace ET.Server
{
    [ChildOf(typeof(SkillManagerComponentS))]
    public class SkillS : Entity, IAwake, IDestroy
    {
        public SkillInfo SkillInfo { get; set; }
        public SkillConfig SkillConfig { get; set; }
        public SkillHandlerS SkillHandlerS { get; set; }
        public SkillState SkillState { get; set; }
        public float SkillLiveTime { get; set; }
        private EntityRef<Unit> theUnitFrom; //来自哪个Unit
        public Unit TheUnitFrom { get => this.theUnitFrom; set => this.theUnitFrom = value; }
        private EntityRef<Unit> theUnitTarget;
        public Unit TheUnitTarget { get => this.theUnitTarget; set => this.theUnitTarget = value; }
        public float3 NowPosition { get; set; } //当前技能的坐标点
        public float3 TargetPosition { get; set; }
        public float LogTime { get; set; } //计时用的
        public float DelayTime { get; set; } //延迟时间
        public float IntervalTime { get; set; } //间隔时间
    }
}