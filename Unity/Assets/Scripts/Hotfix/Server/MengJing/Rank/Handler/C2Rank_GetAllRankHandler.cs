using System;

namespace ET.Server
{
    [MessageHandler(SceneType.Rank)]
    public class C2Rank_GetAllRankHandler : MessageHandler<RankUnit, C2Rank_GetAllRank, Rank2C_GetAllRank>
    {
        protected override async ETTask Run(RankUnit rankUnit, C2Rank_GetAllRank request, Rank2C_GetAllRank response)
        {
            RankCenterComponent rankCenterComponent = rankUnit.Root().GetComponent<RankCenterComponent>();
            int showMaxNum = Math.Min(rankCenterComponent.PlayerRankDataList.Count, ConfigData.ShowRankMaxNum);
            for (int i = 0; i < showMaxNum; i++)
            {
                RankData rankData = rankCenterComponent.PlayerRankDataList[i];
                response.RankDataList.Add(rankData.ToMessage());
            }

            await ETTask.CompletedTask;
        }
    }
}