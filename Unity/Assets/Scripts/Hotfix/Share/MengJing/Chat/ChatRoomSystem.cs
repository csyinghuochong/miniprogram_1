namespace ET
{
    [EntitySystemOf(typeof(ChatRoom))]
    [FriendOf(typeof(ChatRoom))]
    public static partial class ChatRoomSystem
    {
        [EntitySystem]
        private static void Awake(this ChatRoom self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ChatRoom self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ChatRoom self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Chat chat)
                {
                    self.ChatList.Add(chat);
                }
            }
        }

        public static ChatRoomInfo ToMessage(this ChatRoom self)
        {
            ChatRoomInfo chatRoomInfo = ChatRoomInfo.Create();
            chatRoomInfo.ChatRoomKey = self.ChatRoomKey;
            chatRoomInfo.ChatRoomType = self.ChatRoomType;
            chatRoomInfo.UnitList.AddRange(self.UnitList);
            foreach (Chat chat in self.ChatList)
            {
                chatRoomInfo.ChatInfoList.Add(chat.ToMessage());
            }

            return chatRoomInfo;
        }

        public static void FromMessage(this ChatRoom self, ChatRoomInfo chatRoomInfo)
        {
            self.ChatRoomKey = chatRoomInfo.ChatRoomKey;
            self.ChatRoomType = chatRoomInfo.ChatRoomType;
            self.UnitList.AddRange(chatRoomInfo.UnitList);
            foreach (ChatInfo chatInfo in chatRoomInfo.ChatInfoList)
            {
                Chat chat = self.AddChild<Chat>();
                chat.FromMessage(chatInfo);
                self.ChatList.Add(chat);
            }
        }

        public static void AddChatFromMessage(this ChatRoom self, ChatInfo chatInfo)
        {
            Chat chat = self.AddChild<Chat>();
            chat.FromMessage(chatInfo);

            self.ChatList.Add(chat);
        }

        public static void Check(this ChatRoom self)
        {
            if (self.ChatList.Count <= 100)
            {
                return;
            }

            // 保留最新的100条消息，删除最旧的
            int deleteCount = self.ChatList.Count - 100;
            for (int i = 0; i < deleteCount; i++)
            {
                Chat chat = self.ChatList[0];
                chat.Dispose();
                self.ChatList.RemoveAt(0);
            }
        }
    }
}