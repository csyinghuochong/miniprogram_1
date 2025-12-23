namespace ET.Client
{
    public static class ClientChatHelper
    {
        public static async ETTask<int> SendChat(Scene root, string message, ChatChannelType channelType)
        {
            C2Chat_SendChat request = C2Chat_SendChat.Create();
            ChatEntryInfo chatEntryInfo = ChatEntryInfo.Create();
            chatEntryInfo.Content = message;
            chatEntryInfo.Channel = (int)channelType;

            request.ChatEntryInfo = chatEntryInfo;

            Chat2C_SendChat response = (Chat2C_SendChat)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                
            }

            return response.Error;
        }
    }
}