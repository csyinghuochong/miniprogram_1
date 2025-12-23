namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class G2Friend_LoginFriendServerHandler : MessageHandler<Scene, G2Friend_LoginFriendServer, Friend2G_LoginFriendServer>
    {
        protected override async ETTask Run(Scene scene, G2Friend_LoginFriendServer request, Friend2G_LoginFriendServer response)
        {
            FriendUnitComponent friendUnitComponent = scene.GetComponent<FriendUnitComponent>();
            friendUnitComponent.Children.TryGetValue(request.UnitId, out Entity friendUnitEntity);

            FriendUnit friendUnit = friendUnitEntity as FriendUnit;

            if (friendUnit != null)
            {
                return;
            }

            friendUnit = friendUnitComponent.AddChildWithId<FriendUnit>(request.UnitId);
            friendUnit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);

            FriendComponentS friendComponent = await UnitCacheHelper.GetComponent<FriendComponentS>(scene, request.UnitId);
            if (friendComponent == null)
            {
                friendComponent = friendUnit.AddComponent<FriendComponentS>();
            }
            else
            {
                friendUnit.AddComponent(friendComponent);
            }
            
            await friendUnit.AddLocation(LocationType.Friend);

            await ETTask.CompletedTask;
        }
    }
}