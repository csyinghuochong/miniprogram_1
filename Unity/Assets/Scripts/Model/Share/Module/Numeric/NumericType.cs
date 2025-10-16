namespace ET
{
    public static class NumericType
    {
        //最小值，小于此值的都被认为是原始属性
        public const int Min = 0;

        //当前属性[玩家刷新属性的时候不会清掉这些值]
        public const int Now_Hp = 3001;                                         //生命值
        public const int Now_Dead = 3002;                                       //0活 1死
        public const int LastLoginTime = 3003;
        
        public const int Max = 10000;

        public const int Now_MaxHp = 1002;         //生命总值
        public const int Base_MaxHp_Base = Now_MaxHp * 100 + 1;                  //属性累加
        public const int Base_MaxHp_Mul = Now_MaxHp * 100 + 2;                   //属性乘法
        public const int Base_MaxHp_Add = Now_MaxHp * 100 + 3;                   //属性附加
        public const int Extra_Buff_MaxHp_Add = Now_MaxHp * 100 + 11;            //属性Buff附加加法
        public const int Extra_Buff_MaxHp_Mul = Now_MaxHp * 100 + 12;            //属性Buff附加乘法

        public const int Now_MinAct = 1003;         //最低攻击
        public const int Base_MinAct_Base = Now_MinAct * 100 + 1;                 //属性累加
        public const int Base_MinAct_Mul = Now_MinAct * 100 + 2;                  //属性乘法
        public const int Base_MinAct_Add = Now_MinAct * 100 + 3;                  //属性附加
        public const int Extra_Buff_MinAct_Add = Now_MinAct * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MinAct_Mul = Now_MinAct * 100 + 12;           //属性Buff附加乘法

        public const int Now_MaxAct = 1004;         //最高攻击
        public const int Base_MaxAct_Base = Now_MaxAct * 100 + 1;                 //属性累加
        public const int Base_MaxAct_Mul = Now_MaxAct * 100 + 2;                  //属性乘法
        public const int Base_MaxAct_Add = Now_MaxAct * 100 + 3;                  //属性附加
        public const int Extra_Buff_MaxAct_Add = Now_MaxAct * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MaxAct_Mul = Now_MaxAct * 100 + 12;           //属性Buff附加乘法

        public const int Now_MinDef = 1005;         //最低防御
        public const int Base_MinDef_Base = Now_MinDef * 100 + 1;                 //属性累加
        public const int Base_MinDef_Mul = Now_MinDef * 100 + 2;                  //属性乘法
        public const int Base_MinDef_Add = Now_MinDef * 100 + 3;                  //属性附加
        public const int Extra_Buff_MinDef_Add = Now_MinDef * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MinDef_Mul = Now_MinDef * 100 + 12;           //属性Buff附加乘法

        public const int Now_MaxDef = 1006;         //最高防御
        public const int Base_MaxDef_Base = Now_MaxDef * 100 + 1;                 //属性累加
        public const int Base_MaxDef_Mul = Now_MaxDef * 100 + 2;                  //属性乘法
        public const int Base_MaxDef_Add = Now_MaxDef * 100 + 3;                  //属性附加
        public const int Extra_Buff_MaxDef_Add = Now_MaxDef * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MaxDef_Mul = Now_MaxDef * 100 + 12;           //属性Buff附加乘法

        public const int Now_MinAdf = 1007;         //最低魔防
        public const int Base_MinAdf_Base = Now_MinAdf * 100 + 1;                 //属性累加
        public const int Base_MinAdf_Mul = Now_MinAdf * 100 + 2;                  //属性乘法
        public const int Base_MinAdf_Add = Now_MinAdf * 100 + 3;                  //属性附加
        public const int Extra_Buff_MinAdf_Add = Now_MinAdf * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MinAdf_Mul = Now_MinAdf * 100 + 12;           //属性Buff附加乘法

        public const int Now_MaxAdf = 1008;         //最高魔御
        public const int Base_MaxAdf_Base = Now_MaxAdf * 100 + 1;                 //属性累加
        public const int Base_MaxAdf_Mul = Now_MaxAdf * 100 + 2;                  //属性乘法
        public const int Base_MaxAdf_Add = Now_MaxAdf * 100 + 3;                  //属性附加
        public const int Extra_Buff_MaxAdf_Add = Now_MaxAdf * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MaxAdf_Mul = Now_MaxAdf * 100 + 12;           //属性Buff附加乘法

        public const int Now_Speed = 1009;          //当前移动速度
        public const int Base_Speed_Base = Now_Speed * 100 + 1;                 //属性累加
        public const int Base_Speed_Mul = Now_Speed * 100 + 2;                  //属性乘法
        public const int Base_Speed_Add = Now_Speed * 100 + 3;                  //属性附加
        public const int Extra_Buff_Speed_Add = Now_Speed * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Speed_Mul = Now_Speed * 100 + 12;           //属性Buff附加乘法

        public const int Now_Mage = 1010;          //当前法术攻击
        public const int Base_Mage_Base = Now_Mage * 100 + 1;                 //属性累加
        public const int Base_Mage_Mul = Now_Mage * 100 + 2;                  //属性乘法
        public const int Base_Mage_Add = Now_Mage * 100 + 3;                  //属性附加
        public const int Extra_Buff_Mage_Add = Now_Mage * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Mage_Mul = Now_Mage * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_AtkSpeed = 1011;          //当前攻击速度
        public const int Base_AtkSpeed_Base = Now_AtkSpeed * 100 + 1;                 //属性累加
        public const int Base_AtkSpeed_Mul = Now_AtkSpeed * 100 + 2;                  //属性乘法
        public const int Base_AtkSpeed_Add = Now_AtkSpeed * 100 + 3;                  //属性附加
        public const int Extra_Buff_AtkSpeed_Add = Now_AtkSpeed * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_AtkSpeed_Mul = Now_AtkSpeed * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_Critical = 1013;          //当前暴击率
        public const int Base_Critical_Base = Now_Critical * 100 + 1;                 //属性累加
        public const int Base_Critical_Mul = Now_Critical * 100 + 2;                  //属性乘法
        public const int Base_Critical_Add = Now_Critical * 100 + 3;                  //属性附加
        public const int Extra_Buff_Critical_Add = Now_Critical * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Critical_Mul = Now_Critical * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_ResistCritical = 1014;          //当前抗暴击率
        public const int Base_ResistCritical_Base = Now_ResistCritical * 100 + 1;                 //属性累加
        public const int Base_ResistCritical_Mul = Now_ResistCritical * 100 + 2;                  //属性乘法
        public const int Base_ResistCritical_Add = Now_ResistCritical * 100 + 3;                  //属性附加
        public const int Extra_Buff_ResistCritical_Add = Now_ResistCritical * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_ResistCritical_Mul = Now_ResistCritical * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_Evasion = 1015;          //当前闪避率
        public const int Base_Evasion_Base = Now_Evasion * 100 + 1;                 //属性累加
        public const int Base_Evasion_Mul = Now_Evasion * 100 + 2;                  //属性乘法
        public const int Base_Evasion_Add = Now_Evasion * 100 + 3;                  //属性附加
        public const int Extra_Buff_Evasion_Add = Now_Evasion * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Evasion_Mul = Now_Evasion * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_Hit = 1016;          //当前命中率
        public const int Base_Hit_Base = Now_Hit * 100 + 1;                 //属性累加
        public const int Base_Hit_Mul = Now_Hit * 100 + 2;                  //属性乘法
        public const int Base_Hit_Add = Now_Hit * 100 + 3;                  //属性附加
        public const int Extra_Buff_Hit_Add = Now_Hit * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Hit_Mul = Now_Hit * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_DamageAddPro = 1017;          //当前伤害加成
        public const int Base_DamageAddPro_Base = Now_DamageAddPro * 100 + 1;                 //属性累加
        public const int Base_DamageAddPro_Mul = Now_DamageAddPro * 100 + 2;                  //属性乘法
        public const int Base_DamageAddPro_Add = Now_DamageAddPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_DamageAddPro_Add = Now_DamageAddPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_DamageAddPro_Mul = Now_DamageAddPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_DamageLessPro = 1018;          //当前伤害减免
        public const int Base_DamageLessPro_Base = Now_DamageLessPro * 100 + 1;                 //属性累加
        public const int Base_DamageLessPro_Mul = Now_DamageLessPro * 100 + 2;                  //属性乘法
        public const int Base_DamageLessPro_Add = Now_DamageLessPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_DamageLessPro_Add = Now_DamageLessPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_DamageLessPro_Mul = Now_DamageLessPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_HeavyAtkPro = 1019;          //当前重击概率
        public const int Base_HeavyAtkPro_Base = Now_HeavyAtkPro * 100 + 1;                 //属性累加
        public const int Base_HeavyAtkPro_Mul = Now_HeavyAtkPro * 100 + 2;                  //属性乘法
        public const int Base_HeavyAtkPro_Add = Now_HeavyAtkPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_HeavyAtkPro_Add = Now_HeavyAtkPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_HeavyAtkPro_Mul = Now_HeavyAtkPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_HuiXueValue = 1020;          //当前回血值
        public const int Base_HuiXueValue_Base = Now_HuiXueValue * 100 + 1;                 //属性累加
        public const int Base_HuiXueValue_Mul = Now_HuiXueValue * 100 + 2;                  //属性乘法
        public const int Base_HuiXueValue_Add = Now_HuiXueValue * 100 + 3;                  //属性附加
        public const int Extra_Buff_HuiXueValue_Add = Now_HuiXueValue * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_HuiXueValue_Mul = Now_HuiXueValue * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_ArmorHuShi = 1021;          //当前护甲穿透
        public const int Base_ArmorHuShi_Base = Now_ArmorHuShi * 100 + 1;                 //属性累加
        public const int Base_ArmorHuShi_Mul = Now_ArmorHuShi * 100 + 2;                  //属性乘法
        public const int Base_ArmorHuShi_Add = Now_ArmorHuShi * 100 + 3;                  //属性附加
        public const int Extra_Buff_ArmorHuShi_Add = Now_ArmorHuShi * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_ArmorHuShi_Mul = Now_ArmorHuShi * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_SkillCDReduction = 1022;          //当前技能冷却缩减
        public const int Base_SkillCDReduction_Base = Now_SkillCDReduction * 100 + 1;                 //属性累加
        public const int Base_SkillCDReduction_Mul = Now_SkillCDReduction * 100 + 2;                  //属性乘法
        public const int Base_SkillCDReduction_Add = Now_SkillCDReduction * 100 + 3;                  //属性附加
        public const int Extra_Buff_SkillCDReduction_Add = Now_SkillCDReduction * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_SkillCDReduction_Mul = Now_SkillCDReduction * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_CriDamageAddPro = 1023;          //当前暴击伤害加成
        public const int Base_CriDamageAddPro_Base = Now_CriDamageAddPro * 100 + 1;                 //属性累加
        public const int Base_CriDamageAddPro_Mul = Now_CriDamageAddPro * 100 + 2;                  //属性乘法
        public const int Base_CriDamageAddPro_Add = Now_CriDamageAddPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_CriDamageAddPro_Add = Now_CriDamageAddPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_CriDamageAddPro_Mul = Now_CriDamageAddPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_AtkHuShiPro = 1024;          //当前招架概率
        public const int Base_AtkHuShiPro_Base = Now_AtkHuShiPro * 100 + 1;                 //属性累加
        public const int Base_AtkHuShiPro_Mul = Now_AtkHuShiPro * 100 + 2;                  //属性乘法
        public const int Base_AtkHuShiPro_Add = Now_AtkHuShiPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_AtkHuShiPro_Add = Now_AtkHuShiPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_AtkHuShiPro_Mul = Now_AtkHuShiPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_Res = 1025;          //当前韧性概率
        public const int Base_Res_Base = Now_Res * 100 + 1;                 //属性累加
        public const int Base_Res_Mul = Now_Res * 100 + 2;                  //属性乘法
        public const int Base_Res_Add = Now_Res * 100 + 3;                  //属性附加
        public const int Extra_Buff_Res_Add = Now_Res * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Res_Mul = Now_Res * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_SkillDamageTwice = 1026;          //当前法术连击概率
        public const int Base_SkillDamageTwice_Base = Now_SkillDamageTwice * 100 + 1;                 //属性累加
        public const int Base_SkillDamageTwice_Mul = Now_SkillDamageTwice * 100 + 2;                  //属性乘法
        public const int Base_SkillDamageTwice_Add = Now_SkillDamageTwice * 100 + 3;                  //属性附加
        public const int Extra_Buff_SkillDamageTwice_Add = Now_SkillDamageTwice * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_SkillDamageTwice_Mul = Now_SkillDamageTwice * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_GuDingDamageValue = 1027;          //当前固定伤害值
        public const int Base_GuDingDamageValue_Base = Now_GuDingDamageValue * 100 + 1;                 //属性累加
        public const int Base_GuDingDamageValue_Mul = Now_GuDingDamageValue * 100 + 2;                  //属性乘法
        public const int Base_GuDingDamageValue_Add = Now_GuDingDamageValue * 100 + 3;                  //属性附加
        public const int Extra_Buff_GuDingDamageValue_Add = Now_GuDingDamageValue * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_GuDingDamageValue_Mul = Now_GuDingDamageValue * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_FanJiPro = 1028;          //当前反击概率
        public const int Base_FanJiPro_Base = Now_FanJiPro * 100 + 1;                 //属性累加
        public const int Base_FanJiPro_Mul = Now_FanJiPro * 100 + 2;                  //属性乘法
        public const int Base_FanJiPro_Add = Now_FanJiPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_FanJiPro_Add = Now_FanJiPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_FanJiPro_Mul = Now_FanJiPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_XiXuePro = 1029;          //当前汲取概率
        public const int Base_XiXuePro_Base = Now_XiXuePro * 100 + 1;                 //属性累加
        public const int Base_XiXuePro_Mul = Now_XiXuePro * 100 + 2;                  //属性乘法
        public const int Base_XiXuePro_Add = Now_XiXuePro * 100 + 3;                  //属性附加
        public const int Extra_Buff_XiXuePro_Add = Now_XiXuePro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_XiXuePro_Mul = Now_XiXuePro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_FanTanPro = 1030;          //当前反弹概率
        public const int Base_FanTanPro_Base = Now_FanTanPro * 100 + 1;                 //属性累加
        public const int Base_FanTanPro_Mul = Now_FanTanPro * 100 + 2;                  //属性乘法
        public const int Base_FanTanPro_Add = Now_FanTanPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_FanTanPro_Add = Now_FanTanPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_FanTanPro_Mul = Now_FanTanPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_ZhiLiaoEffect = 1031;          //当前治疗效果
        public const int Base_ZhiLiaoEffect_Base = Now_ZhiLiaoEffect * 100 + 1;                 //属性累加
        public const int Base_ZhiLiaoEffect_Mul = Now_ZhiLiaoEffect * 100 + 2;                  //属性乘法
        public const int Base_ZhiLiaoEffect_Add = Now_ZhiLiaoEffect * 100 + 3;                  //属性附加
        public const int Extra_Buff_ZhiLiaoEffect_Add = Now_ZhiLiaoEffect * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_ZhiLiaoEffect_Mul = Now_ZhiLiaoEffect * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_SkillDamageAddPro = 1032;          //当前技能伤害加成
        public const int Base_SkillDamageAddPro_Base = Now_SkillDamageAddPro * 100 + 1;                 //属性累加
        public const int Base_SkillDamageAddPro_Mul = Now_SkillDamageAddPro * 100 + 2;                  //属性乘法
        public const int Base_SkillDamageAddPro_Add = Now_SkillDamageAddPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_SkillDamageAddPro_Add = Now_SkillDamageAddPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_SkillDamageAddPro_Mul = Now_SkillDamageAddPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_AtkMonsterDamageAddPro = 1033;          //当前对怪增伤
        public const int Base_AtkMonsterDamageAddPro_Base = Now_AtkMonsterDamageAddPro * 100 + 1;                 //属性累加
        public const int Base_AtkMonsterDamageAddPro_Mul = Now_AtkMonsterDamageAddPro * 100 + 2;                  //属性乘法
        public const int Base_AtkMonsterDamageAddPro_Add = Now_AtkMonsterDamageAddPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_AtkMonsterDamageAddPro_Add = Now_AtkMonsterDamageAddPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_AtkMonsterDamageAddPro_Mul = Now_AtkMonsterDamageAddPro * 100 + 12;           //属性Buff附加乘法
        
        
        
        public const int AOI = 2204;
        public const int AOIBase = AOI * 10 + 1;
        public const int AOIAdd = AOI * 10 + 2;
        public const int AOIPct = AOI * 10 + 3;
        public const int AOIFinalAdd = AOI * 10 + 4;
        public const int AOIFinalPct = AOI * 10 + 5;
    }
}
