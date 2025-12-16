using System.Collections.Generic;

namespace ET
{
    public static class ConfigData
    {
        #region Box2D默认配置

        public const float DefaultRadius = 0.8f; // 默认半径
        public const float PlayerDensity = 1f; // 玩家密度

        public const float HeroDensity = 10f; // 英雄密度
        public const float HeroLinearDamping = 1f; // 英雄线性阻尼

        public const float MonsterDensity = 30f; // 怪物密度
        public const float MonsterLinearDamping = 1f; // 怪物线性阻尼

        #endregion

        /// <summary>
        /// 0->2D 1->2.5D
        /// </summary>
        [StaticField]
        public static int ViewMode = 0;

        // 2.5D 摄像机角度
        public const float CameraAngle = -25f;

        public const int Item_Gold = 1;
        public const int Item_Diamond = 2;
        public const int Item_Exp = 3;

        [StaticField]
        public static bool LoadSceneFinished;

        public const string RobotPassWord = "et@#robot";

        // 玩家位置同步最大距离 防止速度作弊
        public const float PlayerSynMaxDistance = 3f;

        public const long TransformSyncTime = 200;
        
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

        # region 商店

        /// <summary>
        /// 商店刷新消耗
        /// </summary>
        [StaticField]
        public static List<RewardItem> StoreRefreshCost = new()
        {
            new RewardItem() { ItemId = Item_Diamond, ItemNum = 100 }
        };

        /// <summary>
        /// 商店每天手动刷新次数
        /// </summary>
        [StaticField]
        public static int StoreRefreshNum = 3;

        # endregion
    }
}