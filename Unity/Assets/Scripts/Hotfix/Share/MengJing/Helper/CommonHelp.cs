using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using ET.Client;
using Unity.Mathematics;

namespace ET
{
    public static class CommonHelp
    {
        public const int MaxZone = 1024;

        public static int GetCenterZone()
        {
            return 1000;
        }

        public static bool IsRobotZone(int zone)
        {
            return zone == 1001;
        }

        //版号专区
        public static bool IsBanHaoZone(int zone)
        {
            return zone == 1011;
        }

        //内部专区
        public static bool IsAlphaZone(int zone)
        {
            return zone == 1013;
        }

        public const int Version = 20240130;

        //public static string LocalIp = "192.168.1.16"; 
        public const string LocalIp = "127.0.0.1";

        public const bool AccountOldLogic = true;

        public static int GetMaxBaoShiDu()
        {
            return 120;
        }

        public static int GetSkillCdRate(int sceneType)
        {
            return 1;
        }

        public static int GetDayByTime(long time)
        {
            DateTime dateTime = TimeInfo.Instance.ToDateTime(time);
            return dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;
        }
    }
}