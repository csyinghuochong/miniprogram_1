namespace ET.Client
{
    public static class ClientChatHelper
    {
        public static async ETTask<int> SendChat(Scene root, string message, ChatChannelType channelType)
        {
            C2Chat_SendChat request = C2Chat_SendChat.Create();
            ChatInfo chatInfo = ChatInfo.Create();
            chatInfo.Name = root.GetComponent<UserInfoComponentC>().PlayerName;
            chatInfo.Content = message;
            chatInfo.Channel = (int)channelType;

            request.ChatInfo = chatInfo;

            Chat2C_SendChat response = (Chat2C_SendChat)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                
            }

            return response.Error;
        }
    }
}