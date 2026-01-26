namespace ET.Client
{
    public static class ClientActivityHelper
    {
        public static async ETTask<int> GetAllActivity(Scene root)
        {
            C2M_GetAllActivity request = C2M_GetAllActivity.Create();

            M2C_GetAllActivity response = (M2C_GetAllActivity)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                ActivityRechargePointComponentC activityRechargePointComponent = root.GetComponent<ActivityRechargePointComponentC>();
                activityRechargePointComponent.Clear();
                activityRechargePointComponent.RechargePoint = response.RechargePoint;
                activityRechargePointComponent.ReceivedRechargePointRewardIds.AddRange(response.ReceivedRechargePointRewardIds);
            }

            return response.Error;
        }

        public static async ETTask<int> ActivityRechargePointGetReward(Scene root, int configId)
        {
            C2M_ActivityRechargePointGetReward request = C2M_ActivityRechargePointGetReward.Create();
            request.ConfigId = configId;

            M2C_ActivityRechargePointGetReward response = (M2C_ActivityRechargePointGetReward)await root.GetComponent<ClientSenderComponent>().Call(request);
            if (response.Error == ErrorCode.ERR_Success)
            {
                ActivityRechargePointComponentC activityRechargePointComponent = root.GetComponent<ActivityRechargePointComponentC>();
                activityRechargePointComponent.ReceivedRechargePointRewardIds.Add(configId);
            }
            
            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response.Error;
        }
    }
}