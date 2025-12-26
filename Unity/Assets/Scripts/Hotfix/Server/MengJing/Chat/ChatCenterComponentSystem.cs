namespace ET.Server
{
    [EntitySystemOf(typeof(ChatCenterComponent))]
    [FriendOf(typeof(ChatCenterComponent))]
    public static partial class ChatCenterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ChatCenterComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ChatCenterComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ChatCenterComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is ChatRoom chatRoom)
                {
                    self.ChatRoomDict.Add(chatRoom.ChatRoomKey, chatRoom);
                }
            }
        }
    }
}