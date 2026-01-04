namespace ET.Server
{
    [Invoke((long)SceneType.Rank)]
    public class FiberInit_Rank : AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();
            root.AddComponent<LocationProxyComponent>();
            root.AddComponent<DBManagerComponent>();
            root.AddComponent<MessageLocationSenderComponent>();

            RankCenterComponent rankCenterComponent = await UnitCacheHelper.GetComponent<RankCenterComponent>(root, root.Zone());
            if (rankCenterComponent == null)
            {
                rankCenterComponent = root.AddComponentWithId<RankCenterComponent>(root.Zone());
            }
            else
            {
                root.AddComponent(rankCenterComponent);
            }

            root.AddComponent<RankUnitComponent>();

            await ETTask.CompletedTask;
        }
    }
}