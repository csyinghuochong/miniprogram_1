using System.Collections.Generic;

namespace ET
{
    public static class ConfigData
    {
        public const int Item_Gold = 1;
        public const int Item_Diamond = 2;
        public const int Item_Exp = 3;

        [StaticField]
        public static bool LoadSceneFinished { get; set; }

        public const string RobotPassWord = "et@#robot";

        /// <summary>
        /// 200内部区 201版号区 202中心区 203机器人
        /// </summary>
        [StaticField]
        public static List<int> InnerZoneList = new(4) { 200, 201, 202, 203 };

        /// <summary>
        /// 0 无日志 1 info  2debug  3 waring 4 error
        /// </summary>
        [StaticField]
        public static int LogLevel = 0;

        [StaticField]
        public static List<string> KillInfoList = new();

        [StaticField]
        public static string NoticeLastContent = string.Empty;

        [StaticField]
        public static long NoticeLastGetTime = 0;

        [StaticField]
        public static List<string> LoginInfoList = new();

        [StaticField]
        public static List<string> ZuobiInfoList = new();

        [StaticField]
        public static List<ServerItem> ServerItems = new();

        /// <summary>
        /// 商店刷新时间
        /// </summary>
        public const long StoreRefreshTime = TimeHelper.Hour;
    }
}