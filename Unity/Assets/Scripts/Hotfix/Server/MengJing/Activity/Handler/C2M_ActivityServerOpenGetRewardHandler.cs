using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ActivityServerOpenComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ActivityServerOpenGetRewardHandler : MessageLocationHandler<Unit, C2M_ActivityServerOpenGetReward, M2C_ActivityServerOpenGetReward>
    {
        protected override async ETTask Run(Unit unit, C2M_ActivityServerOpenGetReward request, M2C_ActivityServerOpenGetReward response)
        {
            if (!ServerOpenRewardConfigCategory.Instance.DataMap.ContainsKey(request.ConfigId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            ActivityServerOpenComponent activityServerOpenComponent = unit.GetComponent<ActivityServerOpenComponent>();
            ServerOpenRewardConfig serverOpenRewardConfig = ServerOpenRewardConfigCategory.Instance.Get(request.ConfigId);

            if (activityServerOpenComponent.ReceivedServerOpenRewardIds.Contains(serverOpenRewardConfig.Id))
            {
                response.Error = ErrorCode.ERR_AlreadyReceived;
                return;
            }

            if (serverOpenRewardConfig.RequiredType == 1)
            {
                // 等级要求
                UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();

                if (userInfoComponent.GetLv() < serverOpenRewardConfig.RequiredValue)
                {
                    response.Error = ErrorCode.ERR_NotEnoughLv;
                    return;
                }
            }
            else
            {
                // 战力要求
                NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                if (numericComponent.GetAsInt(NumericType.CombatPower) < serverOpenRewardConfig.RequiredValue)
                {
                    response.Error = ErrorCode.ERR_NotEnoughCombatPower;
                    return;
                }
            }

            activityServerOpenComponent.ReceivedServerOpenRewardIds.Add(serverOpenRewardConfig.Id);

            List<RewardItem> rewardItems = new();
            foreach (var rewardItem in serverOpenRewardConfig.RewardItem)
            {
                rewardItems.Add(rewardItem);
            }

            unit.GetComponent<InventoryComponent>().AddItemData(rewardItems);

            await ETTask.CompletedTask;
        }
    }
}