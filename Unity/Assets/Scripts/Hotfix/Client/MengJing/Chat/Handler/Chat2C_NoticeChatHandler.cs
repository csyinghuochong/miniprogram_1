namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Chat2C_NoticeChatHandler : MessageHandler<Scene, Chat2C_NoticeChat>
    {
        protected override async ETTask Run(Scene root, Chat2C_NoticeChat message)
        {
            root.GetComponent<ChatComponent>().AddChatFromMessage(message.ChatInfo);

            EventSystem.Instance.Publish(root, new ChatUpdate());

            await ETTask.CompletedTask;
        }
    }
}