namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class G2Friend_ExitFriendServerHandler : MessageLocationHandler<FriendUnit, G2Friend_ExitFriendServer, Friend2G_ExitFriendServer>
    {
        protected override async ETTask Run(FriendUnit mailUnit, G2Friend_ExitFriendServer request, Friend2G_ExitFriendServer response)
        {
            FriendComponent friendComponent = mailUnit.GetComponent<FriendComponent>();

            await UnitCacheHelper.SaveComponent(mailUnit.Root(), friendComponent);

            // 通知好友下线
            FriendHelper.FriendOnLineChange(mailUnit.Root(), mailUnit.Id, 0);
            
            FriendUnitExit(mailUnit).Coroutine();

            await ETTask.CompletedTask;
        }

        private async ETTask FriendUnitExit(FriendUnit friendUnit)
        {
            await friendUnit.Fiber().WaitFrameFinish();
            await friendUnit.RemoveLocation(LocationType.Friend);
            friendUnit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Remove(friendUnit.Id);
            friendUnit?.Dispose();
        }
    }
}