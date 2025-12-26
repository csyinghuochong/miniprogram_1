namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    [FriendOf(typeof(ChatUnit))]
    public class C2Chat_SendChatHandler : MessageHandler<ChatUnit, C2Chat_SendChat, Chat2C_SendChat>
    {
        protected override async ETTask Run(ChatUnit chatUnit, C2Chat_SendChat request, Chat2C_SendChat response)
        {
            Scene root = chatUnit.Root();

            using (await root.GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.Chat, chatUnit.Id))
            {
                ChatCenterComponent chatCenterComponent = root.GetComponent<ChatCenterComponent>();
                ChatUnitComponent chatUnitComponent = root.GetComponent<ChatUnitComponent>();

                if (string.IsNullOrEmpty(request.Content))
                {
                    response.Error = ErrorCode.ERR_ChatMessageEmpty;
                    return;
                }

                if (request.Content.Length > ConfigData.ChatContentMax)
                {
                    response.Error = ErrorCode.ERR_ChatContentTooLong;
                    return;
                }

                if (TimeHelper.ServerNow() - chatUnit.LastSendTime < ConfigData.ChatInterval)
                {
                    response.Error = ErrorCode.ERR_ChatTooFast;
                    return;
                }

                if (string.IsNullOrEmpty(request.ChatRoomKey))
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                ChatRoom chatRoom = null;
                if (!chatCenterComponent.ChatRoomDict.TryGetValue(request.ChatRoomKey, out var chatRoomRef))
                {
                    response.Error = ErrorCode.ERR_NotFindChatRoom;
                    Log.Warning($"聊天室不存在: {request.ChatRoomKey}, UnitId: {chatUnit.Id}");
                    return;
                }

                chatRoom = chatRoomRef;

                ChatInfo chatInfo = ChatInfo.Create();
                chatInfo.UnitId = chatUnit.Id;
                chatInfo.SendTime = TimeHelper.ServerNow();
                chatInfo.Name = chatUnit.Name;
                chatInfo.Content = request.Content;

                switch (chatRoom.ChatRoomType)
                {
                    case (int)ChatRoomType.World:
                    {
                        chatRoom.AddChatFromMessage(chatInfo);
                        chatRoom.Check();

                        Chat2C_NoticeChat chat2C_NoticeChat = Chat2C_NoticeChat.Create();
                        chat2C_NoticeChat.ChatRoomKey = chatRoom.ChatRoomKey;
                        chat2C_NoticeChat.ChatInfo = chatInfo;

                        foreach (Entity entity in chatUnitComponent.Children.Values)
                        {
                            ChatUnit targetChatUnit = entity as ChatUnit;

                            MapMessageHelper.SendToClient(chatUnit.Root(), targetChatUnit.Id, chat2C_NoticeChat);
                        }

                        break;
                    }
                    case (int)ChatRoomType.Alliance:
                    case (int)ChatRoomType.Private:
                    {
                        if (!chatRoom.UnitList.Contains(chatUnit.Id))
                        {
                            response.Error = ErrorCode.ERR_NotInChatRoom;
                            Log.Warning($"用户不在聊天室中: RoomKey={request.ChatRoomKey}, UnitId={chatUnit.Id}");
                            return;
                        }

                        if (chatRoom.ChatRoomState != (int)ChatRoomState.Open)
                        {
                            response.Error = ErrorCode.ERR_ChatRoomNotOpen;
                            return;
                        }

                        chatRoom.AddChatFromMessage(chatInfo);
                        chatRoom.Check();

                        Chat2C_NoticeChat chat2C_NoticeChat = Chat2C_NoticeChat.Create();
                        chat2C_NoticeChat.ChatRoomKey = chatRoom.ChatRoomKey;
                        chat2C_NoticeChat.ChatInfo = chatInfo;

                        foreach (long id in chatRoom.UnitList)
                        {
                            if (chatUnitComponent.Children.ContainsKey(id))
                            {
                                MapMessageHelper.SendToClient(chatUnit.Root(), id, chat2C_NoticeChat);
                            }
                        }

                        break;
                    }
                }

                chatUnit.LastSendTime = TimeHelper.ServerNow();
            }

            await ETTask.CompletedTask;
        }
    }
}