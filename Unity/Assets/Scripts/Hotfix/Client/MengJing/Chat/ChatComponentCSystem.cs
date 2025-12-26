using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(ChatComponentC))]
    [FriendOf(typeof(ChatComponentC))]
    public static partial class ChatComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this ChatComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ChatComponentC self)
        {
        }

        public static void AddChatRoomFromMessage(this ChatComponentC self, ChatRoomInfo chatRoomInfo)
        {
            ChatRoom chatRoom = self.AddChild<ChatRoom>();
            chatRoom.FromMessage(chatRoomInfo);

            self.ChatRoomDict.Add(chatRoomInfo.ChatRoomKey, chatRoom);
        }

        public static void Clear(this ChatComponentC self)
        {
            foreach (ChatRoom chatRoom in self.ChatRoomDict.Values)
            {
                chatRoom?.Dispose();
            }

            self.ChatRoomDict.Clear();
        }

        public static List<Chat> GetAllChatList(this ChatComponentC self)
        {
            List<Chat> list = new List<Chat>();
            foreach (ChatRoom chatRoom in self.ChatRoomDict.Values)
            {
                foreach (Chat chat in chatRoom.ChatList)
                {
                    list.Add(chat);
                }
            }

            list.Sort((x, y) => x.SendTime.CompareTo(y.SendTime));

            return list;
        }

        public static List<Chat> GetWorldChatList(this ChatComponentC self)
        {
            List<Chat> list = new List<Chat>();
            foreach (ChatRoom chatRoom in self.ChatRoomDict.Values)
            {
                if (chatRoom.ChatRoomType != (int)ChatRoomType.World)
                {
                    continue;
                }

                foreach (Chat chat in chatRoom.ChatList)
                {
                    list.Add(chat);
                }
            }

            return list;
        }

        public static List<Chat> GetAllianceChatList(this ChatComponentC self)
        {
            List<Chat> list = new List<Chat>();
            foreach (ChatRoom chatRoom in self.ChatRoomDict.Values)
            {
                if (chatRoom.ChatRoomType != (int)ChatRoomType.Alliance)
                {
                    continue;
                }

                foreach (Chat chat in chatRoom.ChatList)
                {
                    list.Add(chat);
                }
            }

            return list;
        }

        public static List<Chat> GetFriendChatList(this ChatComponentC self, long friendId)
        {
            List<Chat> list = new List<Chat>();
            string key = CommonHelp.GetChatRoomKey(self.Root().GetComponent<PlayerInfoComponent>().CurrentRoleId, friendId);
            if (self.ChatRoomDict.ContainsKey(key))
            {
                ChatRoom chatRoom = self.ChatRoomDict[key];
                foreach (Chat chat in chatRoom.ChatList)
                {
                    list.Add(chat);
                }
            }

            return list;
        }
    }
}