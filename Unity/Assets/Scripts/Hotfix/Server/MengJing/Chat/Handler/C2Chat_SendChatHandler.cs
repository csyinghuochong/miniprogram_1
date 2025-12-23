namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    [FriendOf(typeof(ChatUnit))]
    public class C2Chat_SendChatHandler : MessageHandler<ChatUnit, C2Chat_SendChat, Chat2C_SendChat>
    {
        protected override async ETTask Run(ChatUnit chatUnit, C2Chat_SendChat request, Chat2C_SendChat response)
        {
            if (request.ChatEntryInfo == null)
            {
                response.Error = ErrorCode.ERR_ChatInfoNull;
                return;
            }

            if (string.IsNullOrEmpty(request.ChatEntryInfo.Content))
            {
                response.Error = ErrorCode.ERR_ChatMessageEmpty;
                return;
            }

            ChatEntryInfo chatEntryInfo = request.ChatEntryInfo;
            chatEntryInfo.UnitId = chatUnit.Id;

            if (request.ChatEntryInfo.Channel == (int)ChatChannelType.World)
            {
                ChatUnitComponent chatUnitComponent = chatUnit.Root().GetComponent<ChatUnitComponent>();

                foreach (Entity entity in chatUnitComponent.Children.Values)
                {
                    ChatUnit chantUnit = entity as ChatUnit;

                    Chat2C_NoticeChat chat2C_NoticeChat = Chat2C_NoticeChat.Create();
                    chat2C_NoticeChat.ChatEntryInfo = chatEntryInfo;

                    MapMessageHelper.SendToClient(chatUnit.Root(), chantUnit.Id, chat2C_NoticeChat);
                }
            }
            else if (request.ChatEntryInfo.Channel == (int)ChatChannelType.LianMeng)
            {
            }

            await ETTask.CompletedTask;
        }
    }
}