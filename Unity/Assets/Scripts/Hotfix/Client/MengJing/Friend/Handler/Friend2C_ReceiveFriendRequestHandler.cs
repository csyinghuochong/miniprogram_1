namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Friend2C_ReceiveFriendRequestHandler : MessageHandler<Scene, Friend2C_ReceiveFriendRequest>
    {
        protected override async ETTask Run(Scene root, Friend2C_ReceiveFriendRequest message)
        {
            FriendComponentC friendComponent = root.GetComponent<FriendComponentC>();
            friendComponent.AddRequestFromMessage(message.FriendDataInfo);

            EventSystem.Instance.Publish(root, new FriendUpdate());

            await ETTask.CompletedTask;
        }
    }
}