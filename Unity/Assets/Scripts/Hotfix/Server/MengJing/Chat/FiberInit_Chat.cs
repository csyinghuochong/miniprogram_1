namespace ET.Server
{
    [Invoke((long)SceneType.Chat)]
    public class FiberInit_Chat : AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();
            root.AddComponent<LocationProxyComponent>();
            root.AddComponent<DBManagerComponent>();
            root.AddComponent<MessageLocationSenderComponent>();

            ChatCenterComponent chatCenterComponent = await UnitCacheHelper.GetComponent<ChatCenterComponent>(root, root.Zone());
            if (chatCenterComponent == null)
            {
                chatCenterComponent = root.AddComponentWithId<ChatCenterComponent>(root.Zone());
            }
            else
            {
                root.AddComponent(chatCenterComponent);
            }

            string chatRoomKey = ConfigData.WorldChatRoomKey;
            if (!chatCenterComponent.ChatRoomDict.ContainsKey(chatRoomKey))
            {
                // 创建世界聊天室
                ChatRoom chatRoom = chatCenterComponent.AddChild<ChatRoom>();
                chatRoom.ChatRoomKey = chatRoomKey;
                chatRoom.ChatRoomType = (int)ChatRoomType.World;
                chatRoom.ChatRoomState = (int)ChatRoomState.Open;

                chatCenterComponent.ChatRoomDict.Add(chatRoom.ChatRoomKey, chatRoom);

                Log.Info($"创建世界聊天室: {chatRoomKey}, Zone: {root.Zone()}");
            }

            root.AddComponent<ChatUnitComponent>();

            await ETTask.CompletedTask;
        }
    }
}