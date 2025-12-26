namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Chat2C_NoticeChatHandler : MessageHandler<Scene, Chat2C_NoticeChat>
    {
        protected override async ETTask Run(Scene root, Chat2C_NoticeChat message)
        {
            ChatComponentC chatComponent = root.GetComponent<ChatComponentC>();
            if (!chatComponent.ChatRoomDict.TryGetValue(message.ChatRoomKey, out var chatRoomRef))
            {
                Log.Error($"没有该聊天室: {message.ChatRoomKey}");
                return;
            }

            ChatRoom chatRoom = chatRoomRef;
            chatRoom.AddChatFromMessage(message.ChatInfo);

            EventSystem.Instance.Publish(root, new ChatUpdate());

            await ETTask.CompletedTask;
        }
    }
}