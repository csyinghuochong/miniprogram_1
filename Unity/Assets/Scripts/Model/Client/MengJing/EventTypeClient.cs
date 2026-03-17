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

    public struct InitEffectData
    {
        public int EffectTypeEnum;
        public int EffectId;
        public float3 EffectPosition;
        public float EffectAngle;
        public float TargetAngle;
        public long TargetId;
        public long InstanceId;

        // 显示范围 测试
        public float Radius;
    }

    public struct OnUseSkill
    {
        public Unit Unit;
        public int SkillConfigId;
    }

    public struct SkillEffect
    {
        public InitEffectData InitEffectData;
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

    public struct SkillEffectReset
    {
        public long EffectInstanceId;
        public Unit Unit;
    }

    public struct AddBuff
    {
        public Unit Unit;
        public long BuffId;
    }

    public struct FsmChange
    {
        public int FsmHandlerType;
        public int SkillId;
        public Unit Unit;
    }

    public struct StateChange
    {
        public M2C_UnitStateUpdate m2C_UnitStateUpdate;
        public Unit Unit;
    }

    public struct TaskUpdate
    {
    }

    // 任务提交成功
    public struct TaskCommit
    {
        public int TaskConfigId;
    }

    public struct SkillSound
    {
        public string Asset;
    }

    public struct MailUpdate
    {
    }

    public struct ShowTip
    {
        public string Tip;
    }
    
    public struct ChatUpdate
    {
    }
    
    public struct FriendUpdate
    {
    }
    
    public struct ArchiveHeroUpdate
    {
    }
}