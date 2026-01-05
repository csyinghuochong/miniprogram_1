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
                foreach (RankData rankData in self.PlayerRankDataList)
                {
                    if (rankData.UnitId == rankDataInfoList[i].UnitId)
                    {
                        rankData.FromMessage(rankDataInfoList[i]);
                        break;
                    }
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
    }
}