namespace ET
{
    [EntitySystemOf(typeof(Chat))]
    [FriendOf(typeof(Chat))]
    public static partial class ChatSystem
    {
        [EntitySystem]
        private static void Awake(this Chat self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Chat self)
        {
        }

        public static ChatInfo ToMessage(this Chat self)
        {
            ChatInfo chatInfo = ChatInfo.Create();
            chatInfo.UnitId = self.UnitId;
            chatInfo.Name = self.Name;
            chatInfo.Content = self.Content;

            return chatInfo;
        }

        public static void FromMessage(this Chat self, ChatInfo chatInfo)
        {
            self.UnitId = chatInfo.UnitId;
            self.Name = chatInfo.Name;
            self.Content = chatInfo.Content;
        }
    }
}