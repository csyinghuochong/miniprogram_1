using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(ActivityMonthSignInComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ActivityMonthSignInHandler : MessageLocationHandler<Unit, C2M_ActivityMonthSignIn, M2C_ActivityMonthSignIn>
    {
        protected override async ETTask Run(Unit unit, C2M_ActivityMonthSignIn request, M2C_ActivityMonthSignIn response)
        {
            ActivityMonthSignInComponent activityMonthSignInComponent = unit.GetComponent<ActivityMonthSignInComponent>();
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            long nowTime = TimeHelper.ServerNow();
            if (!TimeHelper.IsLaterDay(activityMonthSignInComponent.LastSignInTime, nowTime))
            {
                response.Error = ErrorCode.ERR_AlreadySignIn;
                return;
            }

            int allDay = 0;
            foreach (MonthSignInConfig config in MonthSignInConfigCategory.Instance.DataList)
            {
                if (config.SignInType == 1)
                {
                    allDay = config.RequiredDay;
                }
            }

            if (activityMonthSignInComponent.TotalSignInDay >= allDay)
            {
                response.Error = ErrorCode.ERR_FinishSignIn;
                return;
            }

            activityMonthSignInComponent.LastSignInTime = nowTime;
            activityMonthSignInComponent.TotalSignInDay++;

            MonthSignInConfig monthSignInConfig = null;
            foreach (MonthSignInConfig config in MonthSignInConfigCategory.Instance.DataList)
            {
                if (config.SignInType == 1 && config.RequiredDay == activityMonthSignInComponent.TotalSignInDay)
                {
                    monthSignInConfig = config;

                    break;
                }
            }

            if (monthSignInConfig == null)
            {
                return;
            }

            List<RewardItem> rewardItems = new List<RewardItem>();
            rewardItems.Add(monthSignInConfig.RewardItem);

            inventoryComponent.AddItemData(rewardItems);

            await ETTask.CompletedTask;
        }
    }
}