using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Rank)]
    public class C2Rank_GetAllRankHandler : MessageHandler<RankUnit, C2Rank_GetAllRank, Rank2C_GetAllRank>
    {
        protected override async ETTask Run(RankUnit rankUnit, C2Rank_GetAllRank request, Rank2C_GetAllRank response)
        {
            RankCenterComponent rankCenterComponent = rankUnit.Root().GetComponent<RankCenterComponent>();
            int showMaxNum = Math.Min(rankCenterComponent.PlayerCombatPowerRankList.Count, ConfigData.ShowRankMaxNum);
            for (int i = 0; i < showMaxNum; i++)
            {
                PlayerCombatPowerRank playerCombatPowerRank = rankCenterComponent.PlayerCombatPowerRankList[i];
                response.PlayerCombatPowerRankInfoList.Add(playerCombatPowerRank.ToMessage());
            }

            showMaxNum = Math.Min(rankCenterComponent.AllianceRankList.Count, ConfigData.ShowRankMaxNum);
            for (int i = 0; i < showMaxNum; i++)
            {
                AllianceRank allianceRank = rankCenterComponent.AllianceRankList[i];
                response.AllianceRankInfoList.Add(allianceRank.ToMessage());
            }

            // 测试
            AllianceRankInfo allianceRankInfo1 = AllianceRankInfo.Create();
            allianceRankInfo1.Sort = 1;
            allianceRankInfo1.AllianceId = 1;
            allianceRankInfo1.AllianceName = "测试_家族_1";
            allianceRankInfo1.Active = 100;

            AllianceRankInfo allianceRankInfo2 = AllianceRankInfo.Create();
            allianceRankInfo2.Sort = 2;
            allianceRankInfo2.AllianceId = 2;
            allianceRankInfo2.AllianceName = "测试_家族_2";
            allianceRankInfo2.Active = 50;
            response.AllianceRankInfoList.Add(allianceRankInfo1);
            response.AllianceRankInfoList.Add(allianceRankInfo2);

            await ETTask.CompletedTask;
        }
    }
}