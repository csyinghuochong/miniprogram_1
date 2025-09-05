using System.Collections.Generic;

namespace ET
{
    
    public static class NumericData
    {
    
         //需要广播的数值类型
         [StaticField]
         public static List<int> BroadcastType = new List<int>
         {
             NumericType.Now_Hp,
             NumericType.Now_Dead,
             NumericType.Now_Speed,
             NumericType.Now_MaxHp,
         };

         //1 整数  2 浮点数
         [StaticField]
         public static Dictionary<int, int> NumericValueType = new Dictionary<int, int>
         {
             { (int)NumericType.Base_MaxHp_Base, 1 },
             /*
             { (int)NumericType.Now_Cri, 2 },
             { (int)NumericType.Base_Cri_Base, 2 },
             { (int)NumericType.Base_Cri_Add, 2 },
             { (int)NumericType.Extra_Buff_Cri_Mul, 2 },
             { (int)NumericType.Base_Hit_Base, 2 },
             { (int)NumericType.Base_Hit_Add, 2 },
             { (int)NumericType.Extra_Buff_Hit_Mul, 2 },
             { (int)NumericType.Base_Dodge_Base, 2 },
             { (int)NumericType.Base_Dodge_Add, 2 },
             { (int)NumericType.Extra_Buff_Dodge_Mul, 2 },
             { (int)NumericType.Base_Res_Base, 2 },
             { (int)NumericType.Base_Res_Add, 2 },
             { (int)NumericType.Extra_Buff_Res_Mul, 2 },
             { (int)NumericType.Base_ActDamgeAddPro_Base, 2 },
             { (int)NumericType.Base_ActDamgeAddPro_Add, 2 },
             { (int)NumericType.Extra_Buff_ActDamgeAddPro_Mul, 2 },
             { (int)NumericType.Base_MageDamgeAddPro_Base, 2 },
             { (int)NumericType.Base_MageDamgeAddPro_Add, 2 },
             { (int)NumericType.Extra_Buff_MageDamgeAddPro_Mul, 2 },
             { (int)NumericType.Base_ActDamgeSubPro_Base, 2 },
             { (int)NumericType.Base_ActDamgeSubPro_Add, 2 },
             { (int)NumericType.Extra_Buff_ActDamgeSubPro_Mul, 2 },
             { (int)NumericType.Base_ActDamgeSubPro_Base, 2 },
             { (int)NumericType.Base_ActDamgeSubPro_Add, 2 },
             { (int)NumericType.Extra_Buff_ActDamgeSubPro_Mul, 2 },
             { (int)NumericType.Base_DamgeAddPro_Base, 2 },
             { (int)NumericType.Base_DamgeAddPro_Add, 2 },
             { (int)NumericType.Extra_Buff_DamgeAddPro_Mul, 2 },
             { (int)NumericType.Base_DamgeSubPro_Base, 2 },
             { (int)NumericType.Base_DamgeSubPro_Add, 2 },
             { (int)NumericType.Extra_Buff_DamgeSubPro_Mul, 2 },
             { (int)NumericType.Base_ZhongJiPro_Base, 2 },
             { (int)NumericType.Base_ZhongJiPro_Add, 2 },
             { (int)NumericType.Extra_Buff_ZhongJiPro_Mul, 2 },
             { (int)NumericType.Base_HuShiActPro_Base, 2 },
             { (int)NumericType.Base_HuShiActPro_Add, 2 },
             { (int)NumericType.Extra_Buff_HuShiActPro_Mul, 2 },
             { (int)NumericType.Base_HuShiMagePro_Base, 2 },
             { (int)NumericType.Base_HuShiMagePro_Add, 2 },
             { (int)NumericType.Extra_Buff_HuShiMagePro_Mul, 2 },
             { (int)NumericType.Base_XiXuePro_Base, 2 },
             { (int)NumericType.Base_XiXuePro_Add, 2 },
             { (int)NumericType.Extra_Buff_XiXuePro_Mul, 2 },
             */
         };
    }
}
