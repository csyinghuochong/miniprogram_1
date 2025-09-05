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

        public const int Now_MaxHp = 1002;                                       //生命总值
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
        
        public const int AOI = 2204;
        public const int AOIBase = AOI * 10 + 1;
        public const int AOIAdd = AOI * 10 + 2;
        public const int AOIPct = AOI * 10 + 3;
        public const int AOIFinalAdd = AOI * 10 + 4;
        public const int AOIFinalPct = AOI * 10 + 5;
    }
}
