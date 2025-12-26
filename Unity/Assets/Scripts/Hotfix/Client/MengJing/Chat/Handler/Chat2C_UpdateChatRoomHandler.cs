namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class Chat2C_UpdateChatRoomHandler : MessageHandler<Scene, Chat2C_UpdateChatRoom>
    {
        protected override async ETTask Run(Scene root, Chat2C_UpdateChatRoom message)
        {
            root.GetComponent<ChatComponentC>().AddChatRoomFromMessage(message.ChatRoomInfo);

            await ETTask.CompletedTask;
        }
    }
}