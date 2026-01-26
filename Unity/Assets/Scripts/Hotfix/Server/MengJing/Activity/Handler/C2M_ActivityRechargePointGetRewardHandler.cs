using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ActivityRechargePointComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ActivityRechargePointGetRewardHandler : MessageLocationHandler<Unit, C2M_ActivityRechargePointGetReward, M2C_ActivityRechargePointGetReward>
    {
        protected override async ETTask Run(Unit unit, C2M_ActivityRechargePointGetReward request, M2C_ActivityRechargePointGetReward response)
        {
            ActivityRechargePointComponent activityRechargePointComponent = unit.GetComponent<ActivityRechargePointComponent>();

            if (!RechargePointsRewardConfigCategory.Instance.DataMap.ContainsKey(request.ConfigId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            if (activityRechargePointComponent.ReceivedRechargePointRewardIds.Contains(request.ConfigId))
            {
                response.Error = ErrorCode.ERR_AlreadyReceived;
                return;
            }
            
            RechargePointsRewardConfig rechargePointsRewardConfig = RechargePointsRewardConfigCategory.Instance.DataMap[request.ConfigId];
            if (activityRechargePointComponent.RechargePoint < rechargePointsRewardConfig.RequiredPoints)
            {
                response.Error = ErrorCode.ERR_NotEnoughRechargePoint;
                return;
            }

            activityRechargePointComponent.ReceivedRechargePointRewardIds.Add(request.ConfigId);

            List<RewardItem> rewardItems = new();
            foreach (RewardItem rewardItem in rechargePointsRewardConfig.RewardItem)
            {
                rewardItems.Add(rewardItem);
            }
            unit.GetComponent<InventoryComponent>().AddItemData(rewardItems);

            await ETTask.CompletedTask;
        }
    }
}