namespace ET.Client
{
    public static class ClientChatHelper
    {
        public static async ETTask<int> SendChat(Scene root, string content, ChatRoomType roomType, long targetId = 0)
        {
            string chatRoomKey = null;
            if (roomType == ChatRoomType.World)
            {
                chatRoomKey = ConfigData.WorldChatRoomKey;
            }
            else if (roomType == ChatRoomType.Alliance)
            {
                chatRoomKey = "联盟Id";
            }
            else if (roomType == ChatRoomType.Private)
            {
                chatRoomKey = CommonHelp.GetChatRoomKey(root.GetComponent<PlayerInfoComponent>().CurrentRoleId, targetId);
            }

            C2Chat_SendChat request = C2Chat_SendChat.Create();
            request.ChatRoomKey = chatRoomKey;
            request.Content = content;

            Chat2C_SendChat response = (Chat2C_SendChat)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_ChatMute)
            {
                EventSystem.Instance.Publish(root, new ShowTip() { Tip = response.Message });
            }
            
            return response.Error;
        }

        public static async ETTask<int> GetAllChatRoom(Scene root)
        {
            C2Chat_GetAllChat request = C2Chat_GetAllChat.Create();

            Chat2C_GetAllChat response = (Chat2C_GetAllChat)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                ChatComponentC chatComponent = root.GetComponent<ChatComponentC>();
                chatComponent.Clear();

                foreach (ChatRoomInfo chatRoomInfo in response.ChatRoomInfoList)
                {
                    chatComponent.AddChatRoomFromMessage(chatRoomInfo);
                }
            }

            return response.Error;
        }

        public static async ETTask<int> Report(Scene root, long unitId)
        {
            C2Chat_Report request = C2Chat_Report.Create();
            request.UnitId = unitId;

            Chat2C_Report response = (Chat2C_Report)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }
    }
}