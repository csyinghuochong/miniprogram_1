namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_ReceivedArchiveRewardHandler : MessageLocationHandler<Unit, C2M_ReceivedArchiveReward, M2C_ReceivedArchiveReward>
    {
        protected override async ETTask Run(Unit unit, C2M_ReceivedArchiveReward request, M2C_ReceivedArchiveReward response)
        {
            await ETTask.CompletedTask;
        }
    }
}