using System.Numerics;
using Unity.Mathematics;

namespace ET
{
    public enum BuffState
    {
        None,
        Running,
        Finished,
    }

    /// <summary>
    /// Buff逻辑类型
    /// </summary>
    public static class EffectTypeEnum
    {
        public const int SkillEffect = 1; //技能特效
        public const int BuffEffect = 2;
    }

    public struct InitBuffData
    {
        //buff角度
        public int TargetAngle;

        public float BuffEndTime;

        public string Spellcaster;
        public int UnitType;
        public int UnitConfigId;
        public int SkillConfigId;
        public int BuffConfigId;
        public long UnitIdFrom;
        public float3 TargetPostion;
    }

    public struct InitSkillData
    {
        public int SkillConfigId;
        public long TargetId;
        public float Angle;
        public float3 TargetPosition;
    }

    [EnableClass]
    public class SkillCDItem
    {
        public int SkillConfigId;
        public float CD;
    }

    public enum SkillState
    {
        Running, //正在执行
        Finished, //完成
    }
}