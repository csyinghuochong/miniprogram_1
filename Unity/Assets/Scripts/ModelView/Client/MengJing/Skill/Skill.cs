using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public struct SkillInfo
    {
        public int SkillConfigId;
        public long TargetID;
        public float TargetAngle;
        public Vector3 TargetPosition;
    }

    [EnableClass]
    public class SkillCDItem
    {
        public int SkillConfigId;
        public float CD;
    }

    public enum SkillState
    {
        Waiting, //等待执行
        Running, //正在执行
        Finished, //完成
    }

    public enum SkillActType
    {
        Normal = 0, //普通攻击
        Active = 1, //主动技能
        Passive = 2, //被动
    }

    [ChildOf(typeof(SkillManagerComponent))]
    public class Skill : Entity, IAwake, IDestroy
    {
        public SkillInfo SkillInfo { get; set; }
        public SkillConfig SkillConfig { get; set; }
        public SkillHandler SkillHandler { get; set; }
        public SkillState SkillState { get; set; }
        public float SkillLiveTime { get; set; }
        private EntityRef<Unit> theUnitFrom; //来自哪个Unit
        public Unit TheUnitFrom { get => this.theUnitFrom; set => this.theUnitFrom = value; }
        private EntityRef<Unit> theUnitTarget;
        public Unit TheUnitTarget { get => this.theUnitTarget; set => this.theUnitTarget = value; }
        public Vector3 NowPosition { get; set; } //当前技能的坐标点
        public Vector3 TargetPosition { get; set; }
        public float LogTime { get; set; } //计时用的
        public float DelayTime { get; set; } //延迟时间
        public float IntervalTime { get; set; } //间隔时间
        public bool HasDealtDamage { get; set; }

        public string EffectPath { get; set; }
        public GameObject EffectGameObject { get; set; }
    }
}