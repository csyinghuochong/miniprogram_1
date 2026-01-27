namespace ET.Server
{
    [FriendOf(typeof(ActivityRechargePointComponent))]
    [FriendOf(typeof(ActivityMonthSignInComponent))]
    [FriendOf(typeof(ActivityServerOpenComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GetAllActivityHandler : MessageLocationHandler<Unit, C2M_GetAllActivity, M2C_GetAllActivity>
    {
        protected override async ETTask Run(Unit unit, C2M_GetAllActivity request, M2C_GetAllActivity response)
        {
            ActivityRechargePointComponent activityRechargePointComponent = unit.GetComponent<ActivityRechargePointComponent>();
            response.RechargePoint = activityRechargePointComponent.RechargePoint;
            response.ReceivedRechargePointRewardIds.AddRange(activityRechargePointComponent.ReceivedRechargePointRewardIds);

            ActivityMonthSignInComponent activityMonthSignInComponent = unit.GetComponent<ActivityMonthSignInComponent>();
            response.LastSignInTime = activityMonthSignInComponent.LastSignInTime;
            response.TotalSignInDay = activityMonthSignInComponent.TotalSignInDay;
            response.ReceivedMonthSignInIds.AddRange(activityMonthSignInComponent.ReceivedMonthSignInIds);

            ActivityServerOpenComponent activityServerOpenComponent = unit.GetComponent<ActivityServerOpenComponent>();
            response.ReceivedServerOpenRewardIds.AddRange(activityServerOpenComponent.ReceivedServerOpenRewardIds);

            await ETTask.CompletedTask;
        }
    }
}