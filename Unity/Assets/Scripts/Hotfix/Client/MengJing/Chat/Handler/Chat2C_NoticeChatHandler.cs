namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Chat2C_NoticeChatHandler : MessageHandler<Scene, Chat2C_NoticeChat>
    {
        protected override async ETTask Run(Scene root, Chat2C_NoticeChat message)
        {
            root.AddComponent<ChatComponent>().AddChatFromMessage(message.ChatEntryInfo);

            EventSystem.Instance.Publish(root, new UpdateChatInfo());

            await ETTask.CompletedTask;
        }
    }
}