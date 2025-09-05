using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
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
}