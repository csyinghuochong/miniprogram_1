namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_ActivityRechargePointUpdateHandler : MessageHandler<Scene, M2C_ActivityRechargePointUpdate>
    {
        protected override async ETTask Run(Scene root, M2C_ActivityRechargePointUpdate message)
        {
            ActivityRechargePointComponentC activityRechargePointComponent = root.GetComponent<ActivityRechargePointComponentC>();
            activityRechargePointComponent.RechargePoint = message.RechargePoint;

            await ETTask.CompletedTask;
        }
    }
}