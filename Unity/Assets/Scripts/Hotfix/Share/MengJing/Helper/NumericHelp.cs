using System;
using System.Collections.Generic;

namespace ET
{
    public static class NumericHelp
    {
        /// <summary>
        /// 1 标识整数  2表示浮点数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetNumericValueType(int key)
        {
            if (key == 1009)
            {
                return 2;
            }

            if (key == 3001)
            {
                return 1;
            }

            //增加
            if (key >= 200001 && key < 300000)
            {
                return 2;
            }

            if (key >= 2001 && key < 3000 || key >= 200001 && key < 300000)
                    //if (key >= 1001 && key < 3000)
            {
                return 2;
            }
            else
            {
                if (key >= 100000 && key < 200000)
                {
                    string str = key.ToString().Substring(key.ToString().Length - 1, 1);
                    if (str == "2")
                    {
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }

                //最后排除
                int value = 1;
                NumericData.NumericValueType.TryGetValue(key, out value);
                if (value == 0)
                    return 1;
                return value;

                //return 0;
            }
        }

        //传入值和类型返回对应值
        public static int NumericValueSaveType(int key, float value)
        {
            if (GetNumericValueType(key) == 2)
            {
                return (int)(value * 10000);
            }
            else
            {
                return (int)(value);
            }
        }

        //传入子值返回母值
        public static int ReturnNumParValue(int sonValue)
        {
            return (int)(sonValue / 100);
        }
    }
}