namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Friend2C_FriendOnLineChangeHandler : MessageHandler<Scene, Friend2C_FriendOnLineChange>
    {
        protected override async ETTask Run(Scene root, Friend2C_FriendOnLineChange message)
        {
            FriendComponentC friendComponent = root.GetComponent<FriendComponentC>();
            friendComponent.FriendOnLineChange(message.UnitId, message.OnLine);

            EventSystem.Instance.Publish(root, new FriendUpdate());

            await ETTask.CompletedTask;
        }
    }
}