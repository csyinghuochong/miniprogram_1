using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ActivityMonthSignInComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ActivityMonthSignInTotalHandler : MessageLocationHandler<Unit, C2M_ActivityMonthSignInTotal, M2C_ActivityMonthSignInTotal>
    {
        protected override async ETTask Run(Unit unit, C2M_ActivityMonthSignInTotal request, M2C_ActivityMonthSignInTotal response)
        {
            if (!MonthSignInConfigCategory.Instance.DataMap.ContainsKey(request.ConfigId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            MonthSignInConfig monthSignInConfig = MonthSignInConfigCategory.Instance.Get(request.ConfigId);

            if (monthSignInConfig.SignInType != 2)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            ActivityMonthSignInComponent activityMonthSignInComponent = unit.GetComponent<ActivityMonthSignInComponent>();
            if (activityMonthSignInComponent.ReceivedMonthSignInIds.Contains(monthSignInConfig.Id))
            {
                response.Error = ErrorCode.ERR_AlreadyReceived;
                return;
            }

            if (activityMonthSignInComponent.TotalSignInDay < monthSignInConfig.RequiredDay)
            {
                response.Error = ErrorCode.ERR_NotEnoughSignInDay;
                return;
            }

            activityMonthSignInComponent.ReceivedMonthSignInIds.Add(monthSignInConfig.Id);

            List<RewardItem> rewardItems = new();
            rewardItems.Add(monthSignInConfig.RewardItem);

            unit.GetComponent<InventoryComponent>().AddItemData(rewardItems);

            await ETTask.CompletedTask;
        }
    }
}