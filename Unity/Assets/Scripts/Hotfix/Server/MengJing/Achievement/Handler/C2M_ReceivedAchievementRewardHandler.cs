using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(AchievementComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ReceivedAchievementRewardHandler : MessageLocationHandler<Unit, C2M_ReceivedAchievementReward, M2C_ReceivedAchievementReward>
    {
        protected override async ETTask Run(Unit unit, C2M_ReceivedAchievementReward request, M2C_ReceivedAchievementReward response)
        {
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();
            AchievementComponent achievementComponent = unit.GetComponent<AchievementComponent>();

            if (!AchievementRewardConfigCategory.Instance.DataMap.ContainsKey(request.RewardId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            if (achievementComponent.ReceivedAchievementRewardIds.Contains(request.RewardId))
            {
                response.Error = ErrorCode.ERR_AlreadyReceived;
                return;
            }

            int currentPoint = achievementComponent.GetCurrentPoint();
            AchievementRewardConfig achievementRewardConfig = AchievementRewardConfigCategory.Instance.DataMap[request.RewardId];

            if (currentPoint < achievementRewardConfig.RequiredPoints)
            {
                response.Error = ErrorCode.ERR_NotEnoughAchievementPoint;
                return;
            }

            achievementComponent.ReceivedAchievementRewardIds.Add(request.RewardId);

            List<RewardItem> rewardItems = new();
            foreach (RewardItem rewardItem in achievementRewardConfig.RewardItem)
            {
                rewardItems.Add(rewardItem);
            }

            inventoryComponent.AddItemData(rewardItems);

            await ETTask.CompletedTask;
        }
    }
}