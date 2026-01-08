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
            foreach (PlayerCombatPowerRank rankData in self.PlayerCombatPowerRankList)
            {
                rankData?.Dispose();
            }

            self.PlayerCombatPowerRankList.Clear();
        }

        public static void AddPlayerCombatPowerRankFromMessage(this RankComponent self, PlayerCombatPowerRankInfo info)
        {
            PlayerCombatPowerRank playerCombatPowerRank = self.AddChild<PlayerCombatPowerRank>();
            playerCombatPowerRank.FromMessage(info);

            self.PlayerCombatPowerRankList.Add(playerCombatPowerRank);
        }

        public static void AddAllianceRankFromMessage(this RankComponent self, AllianceRankInfo info)
        {
            AllianceRank allianceRank = self.AddChild<AllianceRank>();
            allianceRank.FromMessage(info);

            self.AllianceRankList.Add(allianceRank);
        }

        public static void PlayerCombatPowerRankUpdate(this RankComponent self, List<PlayerCombatPowerRankInfo> infoList)
        {
            for (int i = 0; i < infoList.Count; i++)
            {
                bool isExit = false;
                foreach (PlayerCombatPowerRank rankData in self.PlayerCombatPowerRankList)
                {
                    if (rankData.UnitId == infoList[i].UnitId)
                    {
                        isExit = true;
                        rankData.FromMessage(infoList[i]);
                        break;
                    }
                }

                if (isExit)
                {
                    continue;
                }

                self.AddPlayerCombatPowerRankFromMessage(infoList[i]);
            }

            self.PlayerCombatPowerRankList.Sort((x, y) =>
            {
                PlayerCombatPowerRank xRank = x;
                PlayerCombatPowerRank yRank = y;
                return xRank.Sort.CompareTo(yRank.Sort);
            });
        }

        public static List<PlayerCombatPowerRank> GetPlayerCombatPowerRankList(this RankComponent self)
        {
            int showMaxNum = Math.Min(self.PlayerCombatPowerRankList.Count, ConfigData.ShowRankMaxNum);

            List<PlayerCombatPowerRank> playerCombatPowerRankList = new();

            for (int i = 0; i < showMaxNum; i++)
            {
                PlayerCombatPowerRank playerCombatPowerRank = self.PlayerCombatPowerRankList[i];
                playerCombatPowerRankList.Add(playerCombatPowerRank);
            }

            return playerCombatPowerRankList;
        }

        public static List<AllianceRank> GetAllianceRankList(this RankComponent self)
        {
            int showMaxNum = Math.Min(self.AllianceRankList.Count, ConfigData.ShowRankMaxNum);

            List<AllianceRank> allianceRankList = new();

            for (int i = 0; i < showMaxNum; i++)
            {
                AllianceRank allianceRank = self.AllianceRankList[i];
                allianceRankList.Add(allianceRank);
            }

            return allianceRankList;
        }
    }
}