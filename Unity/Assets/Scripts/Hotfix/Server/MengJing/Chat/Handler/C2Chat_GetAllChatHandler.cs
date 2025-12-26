namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class C2Chat_GetAllChatHandler : MessageHandler<ChatUnit, C2Chat_GetAllChat, Chat2C_GetAllChat>
    {
        protected override async ETTask Run(ChatUnit chatUnit, C2Chat_GetAllChat request, Chat2C_GetAllChat response)
        {
            ChatCenterComponent chatCenterComponent = chatUnit.Root().GetComponent<ChatCenterComponent>();
            ChatComponentS chatComponent = chatUnit.GetComponent<ChatComponentS>();

            // 世界聊天室
            if (!chatCenterComponent.ChatRoomDict.TryGetValue(ConfigData.WorldChatRoomKey, out var chatRoomRef))
            {
                Log.Error($"世界聊天室不存在! UnitId: {chatUnit.Id}");
                response.Error = ErrorCode.ERR_NotFindChatRoom;
                return;
            }

            ChatRoom chatRoom = chatRoomRef;
            response.ChatRoomInfoList.Add(chatRoom.ToMessage());

            // 好友、联盟聊天室
            foreach (string key in chatComponent.ChatRoomKeyList)
            {
                if (chatCenterComponent.ChatRoomDict.TryGetValue(key, out var value))
                {
                    chatRoom = value;

                    if (chatRoom.ChatRoomState != (int)ChatRoomState.Open)
                    {
                        continue;
                    }

                    response.ChatRoomInfoList.Add(chatRoom.ToMessage());
                }
            }

            await ETTask.CompletedTask;
        }
    }
}