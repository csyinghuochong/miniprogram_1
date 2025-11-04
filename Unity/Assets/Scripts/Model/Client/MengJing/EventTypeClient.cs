using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    public struct RelinkSucceed
    {
        public int ErrorCode;
    }

    public struct UpdateUserData
    {
        public UserDataType UserDataType;
        public long OldLong;
        public long NewLong;
        public string OldString;
        public string NewString;
    }

    public struct ShowFlyTip
    {
        /// <summary>
        /// 0 无边框 1 有边框
        /// </summary>
        public int Type;

        public string Str;
    }

    public struct ReddotChange
    {
        public int ReddotType;
        public int Number;
    }

    public struct DataUpdate_UpdateRoleProper
    {
    }

    public struct HeroFormationUpdate
    {
    }

    public struct HeroUpdate
    {
    }

    public struct InventoryUpdate
    {
    }

    public struct UIItemTip_Sell
    {
    }

    public struct UIItemTip_Wear
    {
    }

    public struct UIItemTip_TakeOff
    {
    }

    public struct EffectData
    {
        public int EffectTypeEnum;
        public int EffectId;
        public float3 EffectPosition;
        public float EffectAngle;
        public float TargetAngle;
        public long TargetID;
        public long InstanceId;
    }

    public struct SkillEffect
    {
        public EffectData EffectData;
        public Unit Unit;
    }

    public struct SkillEffectFinish
    {
        public long EffectInstanceId;
        public Unit Unit;
    }

    public struct SkillEffectMove
    {
        public long EffectInstanceId;
        public float3 Postion;
        public float Angle;
        public Unit Unit;
    }

    public struct FsmChange
    {
        public int FsmHandlerType;
        public int SkillId;
        public Unit Unit;
    }
}