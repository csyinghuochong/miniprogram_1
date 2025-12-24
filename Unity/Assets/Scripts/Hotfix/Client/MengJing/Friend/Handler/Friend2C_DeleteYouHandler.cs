namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Friend2C_DeleteYouHandler : MessageHandler<Scene, Friend2C_DeleteYou>
    {
        protected override async ETTask Run(Scene root, Friend2C_DeleteYou message)
        {
            FriendComponentC friendComponent = root.GetComponent<FriendComponentC>();
            friendComponent.DeleteFriend(message.UnitId);

            EventSystem.Instance.Publish(root, new FriendUpdate());

            await ETTask.CompletedTask;
        }
    }
}