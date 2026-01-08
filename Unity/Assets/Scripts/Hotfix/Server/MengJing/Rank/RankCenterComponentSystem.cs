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
                switch (entity)
                {
                    case PlayerCombatPowerRank playerCombatPowerRank:
                        self.PlayerCombatPowerRankList.Add(playerCombatPowerRank);
                        break;
                    case AllianceRank allianceRank:
                        self.AllianceRankList.Add(allianceRank);
                        break;
                }
            }
        }

        public static void UpdatePlayerCombatPowerRank(this RankCenterComponent self, long unitId, string playerName, long combatPower)
        {
            PlayerCombatPowerRank playerCombatPowerRank = null;
            foreach (PlayerCombatPowerRank data in self.PlayerCombatPowerRankList)
            {
                if (data.UnitId == unitId)
                {
                    playerCombatPowerRank = data;
                    break;
                }
            }

            if (playerCombatPowerRank == null)
            {
                playerCombatPowerRank = self.AddChild<PlayerCombatPowerRank>();
                playerCombatPowerRank.UnitId = unitId;
                playerCombatPowerRank.PlayerName = playerName;
                playerCombatPowerRank.CombatPower = combatPower;
                self.PlayerCombatPowerRankList.Add(playerCombatPowerRank);
            }

            if (string.IsNullOrEmpty(playerName))
            {
                playerName = playerCombatPowerRank.PlayerName;
            }

            bool isUpdate = playerCombatPowerRank.PlayerName != playerName || playerCombatPowerRank.CombatPower != combatPower;

            playerCombatPowerRank.PlayerName = playerName;
            playerCombatPowerRank.CombatPower = combatPower;

            self.SortPlayerCombatPowerRank(isUpdate ? unitId : 0);
        }

        private static void SortPlayerCombatPowerRank(this RankCenterComponent self, long updateUnitId)
        {
            self.PlayerCombatPowerRankList.Sort((x, y) =>
            {
                PlayerCombatPowerRank xRank = x;
                PlayerCombatPowerRank yRank = y;
                return yRank.CombatPower.CompareTo(xRank.CombatPower);
            });

            Rank2C_NoticeRankUpdate message = Rank2C_NoticeRankUpdate.Create();

            int showMaxNum = Math.Min(self.PlayerCombatPowerRankList.Count, ConfigData.ShowRankMaxNum);
            for (int i = 0; i < showMaxNum; i++)
            {
                PlayerCombatPowerRank playerCombatPowerRank = self.PlayerCombatPowerRankList[i];
                // 排名变化或者玩家数据变化
                if (playerCombatPowerRank.Sort != i + 1 || playerCombatPowerRank.UnitId == updateUnitId)
                {
                    playerCombatPowerRank.Sort = i + 1;

                    message.PlayerCombatPowerRankInfoList.Add(playerCombatPowerRank.ToMessage());
                }
            }

            if (message.PlayerCombatPowerRankInfoList.Count > 0)
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