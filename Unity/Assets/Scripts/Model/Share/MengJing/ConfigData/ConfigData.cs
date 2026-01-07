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
        public const float PlayerSynMaxDistance = 10f;

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

        # region 聊天

        public const string WorldChatRoomKey = "WorldChatRoom";

        // 聊天内容最大长度
        public const int ChatContentMax = 100;

        // 聊天间隔
        public const long ChatInterval = 3 * 1000;

        // 聊天举报多少次后会被禁言
        public const int ChatReportMax = 5;

        # endregion

        #region 排行

        // 排行榜显示最大数量
        public const int ShowRankMaxNum = 30;

        #endregion

        # region 抽卡

        // 抽卡掉落ID
        public const int LotteryDrawDropId = 2001;

        // 心愿单道具
        [StaticField]
        public static List<int> LotteryDrawLookingForwardHeroIdList = new()
        {
            10002005, 10002006
        };

        // 1次抽卡消耗
        [StaticField]
        public static List<RewardItem> LotteryDrawCost_One = new()
        {
            new RewardItem() { ItemId = Item_Diamond, ItemNum = 100 }
        };

        // 10次抽卡消耗
        [StaticField]
        public static List<RewardItem> LotteryDrawCost_Ten = new()
        {
            new RewardItem() { ItemId = Item_Diamond, ItemNum = 1000 }
        };

        // 抽卡免费刷新时间
        public const long LotteryDrawFreeTime = TimeHelper.OneDay;

        // 抽卡保底(达到这个数后一定必得传说英雄)
        public const int LotteryDrawBaoDi = 50;

        // 抽卡保底掉落ID
        public const int LotteryDrawBaoDiDropId = 2002;

        # endregion

        # region 图鉴奖励

        // 一个英雄加多少积分
        public const int ArchiveHeroAddScore = 10;

        // 英雄的一个星级加多少积分
        public const int ArchiveHeroStarAddScore = 1;

        [StaticField]
        public static Dictionary<int, List<RewardItem>> ArchiveRewardDic = new()
        {
            {
                10,
                new List<RewardItem>
                {
                    new RewardItem() { ItemId = Item_Gold, ItemNum = 100 },
                    new RewardItem() { ItemId = Item_Diamond, ItemNum = 10 },
                    new RewardItem() { ItemId = Item_Exp, ItemNum = 1000 }
                }
            },
            {
                20, new List<RewardItem>
                {
                    new RewardItem() { ItemId = Item_Gold, ItemNum = 200 },
                    new RewardItem() { ItemId = Item_Diamond, ItemNum = 20 },
                    new RewardItem() { ItemId = Item_Exp, ItemNum = 2000 }
                }
            },
            {
                40, new List<RewardItem>
                {
                    new RewardItem() { ItemId = Item_Gold, ItemNum = 200 },
                    new RewardItem() { ItemId = Item_Diamond, ItemNum = 20 },
                    new RewardItem() { ItemId = Item_Exp, ItemNum = 2000 }
                }
            },
            {
                80, new List<RewardItem>
                {
                    new RewardItem() { ItemId = Item_Gold, ItemNum = 200 },
                    new RewardItem() { ItemId = Item_Diamond, ItemNum = 20 },
                    new RewardItem() { ItemId = Item_Exp, ItemNum = 2000 }
                }
            },
        };

        #endregion
    }
}