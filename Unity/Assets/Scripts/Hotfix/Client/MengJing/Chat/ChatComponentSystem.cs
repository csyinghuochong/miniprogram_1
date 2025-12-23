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

        public static void AddChatFromMessage(this ChatComponent self, ChatEntryInfo chatEntryInfo)
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