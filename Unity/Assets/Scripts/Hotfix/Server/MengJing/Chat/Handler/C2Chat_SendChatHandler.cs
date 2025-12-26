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

                if (string.IsNullOrEmpty(request.ChatRoomKey))
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                if (!chatCenterComponent.ChatRoomDict.ContainsKey(request.ChatRoomKey))
                {
                    response.Error = ErrorCode.ERR_NotFindChatRoom;
                    return;
                }

                ChatRoom chatRoom = chatCenterComponent.ChatRoomDict[request.ChatRoomKey];

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
                            ChatUnit chantUnit = entity as ChatUnit;

                            MapMessageHelper.SendToClient(chatUnit.Root(), chantUnit.Id, chat2C_NoticeChat);
                        }

                        break;
                    }
                    case (int)ChatRoomType.Alliance:
                    case (int)ChatRoomType.Private:
                    {
                        if (!chatRoom.UnitList.Contains(chatUnit.Id))
                        {
                            response.Error = ErrorCode.ERR_NotInChatRoom;
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
            }

            await ETTask.CompletedTask;
        }
    }
}