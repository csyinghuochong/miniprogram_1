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

        public static List<ChatEntry> GetChatEntryList(this ChatComponent self, ChatChannelType chatChannelType)
        {
            List<ChatEntry> chatEntryList = new();
            foreach (ChatEntry chatEntry in self.ChatEntryList)
            {
                if (chatEntry.Channel == (int)chatChannelType)
                {
                    chatEntryList.Add(chatEntry);
                }
            }

            return chatEntryList;
        }

        public static void AddChatEntryFromMessage(this ChatComponent self, ChatEntryInfo chatEntryInfo)
        {
            ChatEntry chatEntry = self.AddChild<ChatEntry>();
            chatEntry.FromMessage(chatEntryInfo);

            self.ChatEntryList.Add(chatEntry);
        }

        public static void Clear(this ChatComponent self)
        {
            foreach (ChatEntry chatEntry in self.ChatEntryList)
            {
                chatEntry?.Dispose();
            }

            self.ChatEntryList.Clear();
        }
    }
}