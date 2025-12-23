namespace ET
{
    [EntitySystemOf(typeof(ChatEntry))]
    [FriendOf(typeof(ChatEntry))]
    public static partial class ChatEntrySystem
    {
        [EntitySystem]
        private static void Awake(this ChatEntry self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ChatEntry self)
        {
        }

        public static ChatEntryInfo ToMessage(this ChatEntry self)
        {
            ChatEntryInfo chatEntryInfo = ChatEntryInfo.Create();
            chatEntryInfo.UnitId = self.UnitId;
            chatEntryInfo.Name = self.Name;
            chatEntryInfo.Content = self.Content;
            chatEntryInfo.Channel = self.Channel;

            return chatEntryInfo;
        }

        public static void FromMessage(this ChatEntry self, ChatEntryInfo chatEntryInfo)
        {
            self.UnitId = chatEntryInfo.UnitId;
            self.Name = chatEntryInfo.Name;
            self.Content = chatEntryInfo.Content;
            self.Channel = chatEntryInfo.Channel;
        }
    }
}