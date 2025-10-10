using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public struct SkillInfo
    {
        public long TargetID;
        public int TargetAngle;
        public int SkillConfigId;
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
        public Unit TheUnitFrom { get; set; } //来自哪个Unit
        public Unit TheUnitTarget { get; set; }
        public Vector3 NowPosition { get; set; } //当前技能的坐标点
        public Vector3 TargetPosition { get; set; }

        public string EffectPath { get; set; }
        public GameObject EffectGameObject { get; set; }
    }
}