using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(ChatComponent))]
    [FriendOf(typeof(ChatComponent))]
    public static partial class ChatComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ChatComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ChatComponent self)
        {
        }

        public static List<Chat> GetChatEntryList(this ChatComponent self, ChatChannelType chatChannelType)
        {
            List<Chat> chatList = new();
            foreach (Chat chatEntry in self.ChatList)
            {
                if (chatEntry.Channel == (int)chatChannelType)
                {
                    chatList.Add(chatEntry);
                }
            }

            return chatList;
        }

        public static void AddChatFromMessage(this ChatComponent self, ChatInfo chatInfo)
        {
            Chat chat = self.AddChild<Chat>();
            chat.FromMessage(chatInfo);

            self.ChatList.Add(chat);
        }

        public static void Clear(this ChatComponent self)
        {
            foreach (Chat chat in self.ChatList)
            {
                chat?.Dispose();
            }

            self.ChatList.Clear();
        }
    }
}