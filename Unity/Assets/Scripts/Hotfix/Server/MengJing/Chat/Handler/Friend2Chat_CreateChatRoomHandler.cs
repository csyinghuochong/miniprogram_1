namespace ET.Server
{
    [MessageHandler(SceneType.Chat)]
    public class Friend2Chat_CreateChatRoomHandler : MessageHandler<Scene, Friend2Chat_CreateChatRoom, Chat2Friend_CreateChatRoom>
    {
        protected override async ETTask Run(Scene scene, Friend2Chat_CreateChatRoom request, Chat2Friend_CreateChatRoom response)
        {
            ChatCenterComponent chatCenterComponent = scene.GetComponent<ChatCenterComponent>();
            string chatRoomKey = CommonHelp.GetChatRoomKey(request.UnitId_1, request.UnitId_2);

            if (!chatCenterComponent.ChatRoomDict.ContainsKey(chatRoomKey))
            {
                ChatRoom chatRoom = chatCenterComponent.AddChild<ChatRoom>();
                chatRoom.ChatRoomKey = chatRoomKey;
                chatRoom.ChatRoomType = (int)ChatRoomType.Private;
                chatRoom.UnitList.Add(request.UnitId_1);
                chatRoom.UnitList.Add(request.UnitId_2);

                chatCenterComponent.ChatRoomDict.Add(chatRoom.ChatRoomKey, chatRoom);

                ChatUnitComponent chatUnitComponent = scene.GetComponent<ChatUnitComponent>();
                foreach (long unitId in chatRoom.UnitList)
                {
                    chatUnitComponent.Children.TryGetValue(unitId, out Entity chatUnitEntity);
                    ChatUnit chatUnit = chatUnitEntity as ChatUnit;

                    if (chatUnit != null)
                    {
                        ChatComponentS chatComponent = chatUnit.GetComponent<ChatComponentS>();
                        if (!chatComponent.ChatRoomKeyList.Contains(chatRoomKey))
                        {
                            chatComponent.ChatRoomKeyList.Add(chatRoomKey);

                            // 通知客户端
                            Chat2C_UpdateChatRoom chat2C_UpdateChatRoom = Chat2C_UpdateChatRoom.Create();
                            chat2C_UpdateChatRoom.ChatRoomInfo = chatRoom.ToMessage();

                            MapMessageHelper.SendToClient(scene, unitId, chat2C_UpdateChatRoom);
                        }
                    }
                    else
                    {
                        ChatComponentS chatComponent = await UnitCacheHelper.GetComponent<ChatComponentS>(scene, unitId);
                        if (!chatComponent.ChatRoomKeyList.Contains(chatRoomKey))
                        {
                            chatComponent.ChatRoomKeyList.Add(chatRoomKey);
                        }

                        await UnitCacheHelper.SaveComponent(scene, chatComponent);
                    }
                }
            }

            await ETTask.CompletedTask;
        }
    }
}