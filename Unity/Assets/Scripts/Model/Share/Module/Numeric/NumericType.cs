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
        public const int CombatPower = 3004;                                    //战斗力
        public const int CurrentHeroId = 3005;                            //在主城显示的英雄
        
        public const int Max = 10000;

        public const int Now_MaxHp = 1002;         //生命总值
        public const int Base_MaxHp_Base = Now_MaxHp * 100 + 1;                  //属性累加
        public const int Base_MaxHp_Mul = Now_MaxHp * 100 + 2;                   //属性乘法
        public const int Base_MaxHp_Add = Now_MaxHp * 100 + 3;                   //属性附加
        public const int Extra_Buff_MaxHp_Add = Now_MaxHp * 100 + 11;            //属性Buff附加加法
        public const int Extra_Buff_MaxHp_Mul = Now_MaxHp * 100 + 12;            //属性Buff附加乘法

        public const int Now_MinAct = 1003;          //最低攻击
        public const int Base_MinAct_Base = Now_MinAct * 100 + 1;                 //属性累加
        public const int Base_MinAct_Mul = Now_MinAct * 100 + 2;                  //属性乘法
        public const int Base_MinAct_Add = Now_MinAct * 100 + 3;                  //属性附加
        public const int Extra_Buff_MinAct_Add = Now_MinAct * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MinAct_Mul = Now_MinAct * 100 + 12;           //属性Buff附加乘法

        public const int Now_MaxAct = 1004;          //最高攻击
        public const int Base_MaxAct_Base = Now_MaxAct * 100 + 1;                 //属性累加
        public const int Base_MaxAct_Mul = Now_MaxAct * 100 + 2;                  //属性乘法
        public const int Base_MaxAct_Add = Now_MaxAct * 100 + 3;                  //属性附加
        public const int Extra_Buff_MaxAct_Add = Now_MaxAct * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MaxAct_Mul = Now_MaxAct * 100 + 12;           //属性Buff附加乘法

        public const int Now_MinDef = 1005;          //最低防御
        public const int Base_MinDef_Base = Now_MinDef * 100 + 1;                 //属性累加
        public const int Base_MinDef_Mul = Now_MinDef * 100 + 2;                  //属性乘法
        public const int Base_MinDef_Add = Now_MinDef * 100 + 3;                  //属性附加
        public const int Extra_Buff_MinDef_Add = Now_MinDef * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MinDef_Mul = Now_MinDef * 100 + 12;           //属性Buff附加乘法

        public const int Now_MaxDef = 1006;          //最高防御
        public const int Base_MaxDef_Base = Now_MaxDef * 100 + 1;                 //属性累加
        public const int Base_MaxDef_Mul = Now_MaxDef * 100 + 2;                  //属性乘法
        public const int Base_MaxDef_Add = Now_MaxDef * 100 + 3;                  //属性附加
        public const int Extra_Buff_MaxDef_Add = Now_MaxDef * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MaxDef_Mul = Now_MaxDef * 100 + 12;           //属性Buff附加乘法

        public const int Now_MinAdf = 1007;          //最低魔防
        public const int Base_MinAdf_Base = Now_MinAdf * 100 + 1;                 //属性累加
        public const int Base_MinAdf_Mul = Now_MinAdf * 100 + 2;                  //属性乘法
        public const int Base_MinAdf_Add = Now_MinAdf * 100 + 3;                  //属性附加
        public const int Extra_Buff_MinAdf_Add = Now_MinAdf * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MinAdf_Mul = Now_MinAdf * 100 + 12;           //属性Buff附加乘法

        public const int Now_MaxAdf = 1008;          //最高魔御
        public const int Base_MaxAdf_Base = Now_MaxAdf * 100 + 1;                 //属性累加
        public const int Base_MaxAdf_Mul = Now_MaxAdf * 100 + 2;                  //属性乘法
        public const int Base_MaxAdf_Add = Now_MaxAdf * 100 + 3;                  //属性附加
        public const int Extra_Buff_MaxAdf_Add = Now_MaxAdf * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MaxAdf_Mul = Now_MaxAdf * 100 + 12;           //属性Buff附加乘法

        public const int Now_MoveSpeed = 1009;          //当前移动速度
        public const int Base_Speed_Base = Now_MoveSpeed * 100 + 1;                 //属性累加
        public const int Base_Speed_Mul = Now_MoveSpeed * 100 + 2;                  //属性乘法
        public const int Base_Speed_Add = Now_MoveSpeed * 100 + 3;                  //属性附加
        public const int Extra_Buff_Speed_Add = Now_MoveSpeed * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Speed_Mul = Now_MoveSpeed * 100 + 12;           //属性Buff附加乘法

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
        
        public const int Now_Cri = 1013;          //当前暴击率
        public const int Base_Cri_Base = Now_Cri * 100 + 1;                 //属性累加
        public const int Base_Cri_Mul = Now_Cri * 100 + 2;                  //属性乘法
        public const int Base_Cri_Add = Now_Cri * 100 + 3;                  //属性附加
        public const int Extra_Buff_Cri_Add = Now_Cri * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Cri_Mul = Now_Cri * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_ReCri = 1014;          //当前抗暴击率
        public const int Base_ReCri_Base = Now_ReCri * 100 + 1;                 //属性累加
        public const int Base_ReCri_Mul = Now_ReCri * 100 + 2;                  //属性乘法
        public const int Base_ReCri_Add = Now_ReCri * 100 + 3;                  //属性附加
        public const int Extra_Buff_ReCri_Add = Now_ReCri * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_ReCri_Mul = Now_ReCri * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_Eva = 1015;          //当前闪避率
        public const int Base_Eva_Base = Now_Eva * 100 + 1;                 //属性累加
        public const int Base_Eva_Mul = Now_Eva * 100 + 2;                  //属性乘法
        public const int Base_Eva_Add = Now_Eva * 100 + 3;                  //属性附加
        public const int Extra_Buff_Eva_Add = Now_Eva * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Eva_Mul = Now_Eva * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_Hit = 1016;          //当前命中率
        public const int Base_Hit_Base = Now_Hit * 100 + 1;                 //属性累加
        public const int Base_Hit_Mul = Now_Hit * 100 + 2;                  //属性乘法
        public const int Base_Hit_Add = Now_Hit * 100 + 3;                  //属性附加
        public const int Extra_Buff_Hit_Add = Now_Hit * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_Hit_Mul = Now_Hit * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_AtkDamageAddPro = 1017;          //当前伤害加成
        public const int Base_AtkDamageAddPro_Base = Now_AtkDamageAddPro * 100 + 1;                 //属性累加
        public const int Base_AtkDamageAddPro_Mul = Now_AtkDamageAddPro * 100 + 2;                  //属性乘法
        public const int Base_AtkDamageAddPro_Add = Now_AtkDamageAddPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_AtkDamageAddPro_Add = Now_AtkDamageAddPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_AtkDamageAddPro_Mul = Now_AtkDamageAddPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_HitDamageLessPro = 1018;          //当前伤害减免
        public const int Base_HitDamageLessPro_Base = Now_HitDamageLessPro * 100 + 1;                 //属性累加
        public const int Base_HitDamageLessPro_Mul = Now_HitDamageLessPro * 100 + 2;                  //属性乘法
        public const int Base_HitDamageLessPro_Add = Now_HitDamageLessPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_HitDamageLessPro_Add = Now_HitDamageLessPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_HitDamageLessPro_Mul = Now_HitDamageLessPro * 100 + 12;           //属性Buff附加乘法
        
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
        
        public const int Now_AngerValue = 1034;          //当前怒气值
        public const int Base_AngerValue_Base = Now_AngerValue * 100 + 1;                 //属性累加
        public const int Base_AngerValue_Mul = Now_AngerValue * 100 + 2;                  //属性乘法
        public const int Base_AngerValue_Add = Now_AngerValue * 100 + 3;                  //属性附加
        public const int Extra_Buff_AngerValue_Add = Now_AngerValue * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_AngerValue_Mul = Now_AngerValue * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_MaxAngerValue = 1035;          //最大怒气值
        public const int Base_MaxAngerValue_Base = Now_MaxAngerValue * 100 + 1;                 //属性累加
        public const int Base_MaxAngerValue_Mul = Now_MaxAngerValue * 100 + 2;                  //属性乘法
        public const int Base_MaxAngerValue_Add = Now_MaxAngerValue * 100 + 3;                  //属性附加
        public const int Extra_Buff_MaxAngerValue_Add = Now_MaxAngerValue * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_MaxAngerValue_Mul = Now_MaxAngerValue * 100 + 12;           //属性Buff附加乘法
        
        //----------------技能属性-------------
        
        public const int Now_FuHuoPro = 1300;          //当前复活概率
        public const int Base_FuHuoPro_Base = Now_FuHuoPro * 100 + 1;                 //属性累加
        public const int Base_FuHuoPro_Mul = Now_FuHuoPro * 100 + 2;                  //属性乘法
        public const int Base_FuHuoPro_Add = Now_FuHuoPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_FuHuoPro_Add = Now_FuHuoPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_FuHuoPro_Mul = Now_FuHuoPro * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_SkillEvasion = 1301;          //当前技能闪避率
        public const int Base_SkillEvasion_Base = Now_SkillEvasion * 100 + 1;                 //属性累加
        public const int Base_SkillEvasion_Mul = Now_SkillEvasion * 100 + 2;                  //属性乘法
        public const int Base_SkillEvasion_Add = Now_SkillEvasion * 100 + 3;                  //属性附加
        public const int Extra_Buff_SkillEvasion_Add = Now_SkillEvasion * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_SkillEvasion_Mul = Now_SkillEvasion * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_ShieldValue = 1302;          //当前护盾值
        public const int Base_ShieldValue_Base = Now_ShieldValue * 100 + 1;                 //属性累加
        public const int Base_ShieldValue_Mul = Now_ShieldValue * 100 + 2;                  //属性乘法
        public const int Base_ShieldValue_Add = Now_ShieldValue * 100 + 3;                  //属性附加
        public const int Extra_Buff_ShieldValue_Add = Now_ShieldValue * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_ShieldValue_Mul = Now_ShieldValue * 100 + 12;           //属性Buff附加乘法
        
        //----------------其它属性-------------
        
        public const int Now_AtkDistance = 1500;          //当前攻击距离
        public const int Base_AtkDistance_Base = Now_AtkDistance * 100 + 1;                 //属性累加
        public const int Base_AtkDistance_Mul = Now_AtkDistance * 100 + 2;                  //属性乘法
        public const int Base_AtkDistance_Add = Now_AtkDistance * 100 + 3;                  //属性附加
        public const int Extra_Buff_AtkDistance_Add = Now_AtkDistance * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_AtkDistance_Mul = Now_AtkDistance * 100 + 12;           //属性Buff附加乘法
        
        public const int Now_ExpAddPro = 1501;          //当前经验获取加成
        public const int Base_ExpAddPro_Base = Now_ExpAddPro * 100 + 1;                 //属性累加
        public const int Base_ExpAddPro_Mul = Now_ExpAddPro * 100 + 2;                  //属性乘法
        public const int Base_ExpAddPro_Add = Now_ExpAddPro * 100 + 3;                  //属性附加
        public const int Extra_Buff_ExpAddPro_Add = Now_ExpAddPro * 100 + 11;           //属性Buff附加加法
        public const int Extra_Buff_ExpAddPro_Mul = Now_ExpAddPro * 100 + 12;           //属性Buff附加乘法
        
        //----------------抗性-------------
        
        public const int Now_Resistance_Fire_Pro = 2000;          //当前火焰抗性
        public const int Base_Now_Resistance_Fire_Pro_Base = Now_Resistance_Fire_Pro * 100 + 1;              //属性累加
        public const int Base_Resistance_Fire_Pro_Mul = Now_Resistance_Fire_Pro * 100 + 2;                   //属性乘法
        public const int Base_Resistance_Fire_Pro_Add = Now_Resistance_Fire_Pro * 100 + 3;                   //属性附加
        public const int Extra_Buff_Resistance_Fire_Pro_Add = Now_Resistance_Fire_Pro * 100 + 11;            //属性Buff附加加法
        public const int Extra_Buff_Resistance_Fire_Pro_Mul = Now_Resistance_Fire_Pro * 100 + 12;            //属性Buff附加乘法
        
        public const int Now_Resistance_Shadow_Pro = 2001;          //当前暗影抗性
        public const int Base_Resistance_Shadow_Pro_Base = Now_Resistance_Shadow_Pro * 100 + 1;                  //属性累加
        public const int Base_Resistance_Shadow_Pro_Mul = Now_Resistance_Shadow_Pro * 100 + 2;                   //属性乘法
        public const int Base_Resistance_Shadow_Pro_Add = Now_Resistance_Shadow_Pro * 100 + 3;                   //属性附加
        public const int Extra_Buff_Resistance_Shadow_Pro_Add = Now_Resistance_Shadow_Pro * 100 + 11;            //属性Buff附加加法
        public const int Extra_Buff_Resistance_Shadow_Pro_Mul = Now_Resistance_Shadow_Pro * 100 + 12;            //属性Buff附加乘法
        
        public const int Now_Resistance_Nature_Pro = 2002;          //当前自然抗性
        public const int Base_Resistance_Nature_Pro_Base = Now_Resistance_Nature_Pro * 100 + 1;                  //属性累加
        public const int Base_Resistance_Nature_Pro_Mul = Now_Resistance_Nature_Pro * 100 + 2;                   //属性乘法
        public const int Base_Resistance_Nature_Pro_Add = Now_Resistance_Nature_Pro * 100 + 3;                   //属性附加
        public const int Extra_Buff_Resistance_Nature_Pro_Add = Now_Resistance_Nature_Pro * 100 + 11;            //属性Buff附加加法
        public const int Extra_Buff_Resistance_Nature_Pro_Mul = Now_Resistance_Nature_Pro * 100 + 12;            //属性Buff附加乘法
        
        public const int Now_Resistance_Ice_Pro = 2003;          //当前冰霜抗性
        public const int Base_Resistance_Ice_Pro_Base = Now_Resistance_Ice_Pro * 100 + 1;                  //属性累加
        public const int Base_Resistance_Ice_Pro_Mul = Now_Resistance_Ice_Pro * 100 + 2;                   //属性乘法
        public const int Base_Resistance_Ice_Pro_Add = Now_Resistance_Ice_Pro * 100 + 3;                   //属性附加
        public const int Extra_Buff_Resistance_Ice_Pro_Add = Now_Resistance_Ice_Pro * 100 + 11;            //属性Buff附加加法
        public const int Extra_Buff_Resistance_Ice_Pro_Mul = Now_Resistance_Ice_Pro * 100 + 12;            //属性Buff附加乘法
        
        public const int AOI = 2204;
        public const int AOIBase = AOI * 10 + 1;
        public const int AOIAdd = AOI * 10 + 2;
        public const int AOIPct = AOI * 10 + 3;
        public const int AOIFinalAdd = AOI * 10 + 4;
        public const int AOIFinalPct = AOI * 10 + 5;
    }
}
