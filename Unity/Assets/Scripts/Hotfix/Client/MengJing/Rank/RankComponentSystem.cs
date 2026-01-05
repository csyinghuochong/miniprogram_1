using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(RankComponent))]
    [FriendOf(typeof(RankComponent))]
    public static partial class RankComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RankComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RankComponent self)
        {
        }

        public static void Clear(this RankComponent self)
        {
            foreach (RankData rankData in self.PlayerRankDataList)
            {
                rankData?.Dispose();
            }

            self.PlayerRankDataList.Clear();
        }

        public static void AddRankDataFromMessage(this RankComponent self, RankDataInfo rankDataInfo)
        {
            RankData rankData = self.AddChild<RankData>();
            rankData.FromMessage(rankDataInfo);

            self.PlayerRankDataList.Add(rankData);
        }

        public static void RankUpdate(this RankComponent self, List<RankDataInfo> rankDataInfoList)
        {
            for (int i = 0; i < rankDataInfoList.Count; i++)
            {
                bool isExit = false;
                foreach (RankData rankData in self.PlayerRankDataList)
                {
                    if (rankData.UnitId == rankDataInfoList[i].UnitId)
                    {
                        isExit = true;
                        rankData.FromMessage(rankDataInfoList[i]);
                        break;
                    }
                }

                if (isExit)
                {
                    continue;
                }

                self.AddRankDataFromMessage(rankDataInfoList[i]);
            }

            self.PlayerRankDataList.Sort((x, y) =>
            {
                RankData xData = x;
                RankData yData = y;
                return xData.Rank.CompareTo(yData.Rank);
            });
        }

        public static List<RankData> GetPlayerRankDataList(this RankComponent self)
        {
            int showMaxNum = Math.Min(self.PlayerRankDataList.Count, ConfigData.ShowRankMaxNum);

            List<RankData> rankDataList = new();

            for (int i = 0; i < showMaxNum; i++)
            {
                RankData rankData = self.PlayerRankDataList[i];
                rankDataList.Add(rankData);
            }

            return rankDataList;
        }
    }
}