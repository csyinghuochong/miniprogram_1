namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Friend2C_FriendRequestSucceedHandler : MessageHandler<Scene, Friend2C_FriendRequestSucceed>
    {
        protected override async ETTask Run(Scene root, Friend2C_FriendRequestSucceed message)
        {
            FriendComponentC friendComponent = root.GetComponent<FriendComponentC>();
            friendComponent.AddFriendFromMessage(message.FriendDataInfo);

            EventSystem.Instance.Publish(root, new FriendUpdate());

            await ETTask.CompletedTask;
        }
    }
}