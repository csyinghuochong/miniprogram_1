using System;
using System.Collections.Generic;

namespace ET.Server
{
    [EntitySystemOf(typeof(RankCenterComponent))]
    [FriendOf(typeof(RankCenterComponent))]
    public static partial class RankCenterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RankCenterComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RankCenterComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this RankCenterComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is RankData rankData)
                {
                    if (rankData.RankType == (int)RankType.PlayerRank)
                    {
                        self.PlayerRankDataList.Add(rankData);
                    }
                }
            }
        }

        public static void UpdatePlayerRankData(this RankCenterComponent self, long unitId, string playerName, long combatPower)
        {
            RankData rankData = null;
            foreach (RankData data in self.PlayerRankDataList)
            {
                if (data.UnitId == unitId)
                {
                    rankData = data;
                    break;
                }
            }

            if (rankData == null)
            {
                rankData = self.AddChild<RankData>();
                rankData.UnitId = unitId;
                rankData.PlayerName = playerName;
                rankData.CombatPower = combatPower;
                self.PlayerRankDataList.Add(rankData);
            }

            if (string.IsNullOrEmpty(playerName))
            {
                playerName = rankData.PlayerName;
            }

            bool isUpdate = rankData.PlayerName != playerName || rankData.CombatPower != combatPower;

            rankData.PlayerName = playerName;
            rankData.CombatPower = combatPower;

            self.SortRank(isUpdate ? unitId : 0);
        }

        private static void SortRank(this RankCenterComponent self, long updateUnitId)
        {
            self.PlayerRankDataList.Sort((x, y) =>
            {
                RankData xData = x;
                RankData yData = y;
                return xData.CombatPower.CompareTo(yData.CombatPower);
            });

            Rank2C_NoticeRankUpdate message = Rank2C_NoticeRankUpdate.Create();

            int showMaxNum = Math.Min(self.PlayerRankDataList.Count, ConfigData.ShowRankMaxNum);
            for (int i = 0; i < showMaxNum; i++)
            {
                RankData rankData = self.PlayerRankDataList[i];
                // 排名变化或者玩家数据变化
                if (rankData.Rank != i + 1 || rankData.UnitId == updateUnitId)
                {
                    rankData.Rank = i + 1;

                    message.RankDataInfoList.Add(rankData.ToMessage());
                }
            }

            if (message.RankDataInfoList.Count > 0)
            {
                RankUnitComponent rankUnitComponent = self.Root().GetComponent<RankUnitComponent>();
                foreach (Entity entity in rankUnitComponent.Children.Values)
                {
                    RankUnit rankUnit = entity as RankUnit;

                    MapMessageHelper.SendToClient(rankUnit.Root(), rankUnit.Id, message);
                }
            }
        }
    }
}