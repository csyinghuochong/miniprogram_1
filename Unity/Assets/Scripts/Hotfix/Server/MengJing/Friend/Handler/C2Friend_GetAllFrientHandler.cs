namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_GetAllFriendHandler : MessageHandler<FriendUnit, C2Friend_GetAllFriend, Friend2C_GetAllFriend>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_GetAllFriend request, Friend2C_GetAllFriend response)
        {
            FriendComponentS friendComponent = friendUnit.GetComponent<FriendComponentS>();

            response.FriendList.AddRange(friendComponent.FriendList);
            response.ApplyList.AddRange(friendComponent.ApplyList);
            response.BlackList.AddRange(friendComponent.BlackList);

            await ETTask.CompletedTask;
        }
    }
}