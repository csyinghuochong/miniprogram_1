using Cysharp.Text;
using TMPro;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMainChatItem))]
    [FriendOf(typeof(UIMainChatItem))]
    public static partial class UIMainChatItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIMainChatItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;
            self.Text_ChatType = rc.Get<GameObject>("Text_ChatType").GetComponent<TMP_Text>();
            self.Text_Content = rc.Get<GameObject>("Text_Content").GetComponent<TMP_Text>();
        }

        public static void UpdateInfo(this UIMainChatItem self, Chat chat)
        {
            self.Chat = chat;

            ChatRoom chatRoom = chat.GetParent<ChatRoom>();

            switch (chatRoom.ChatRoomType)
            {
                case (int)ChatRoomType.World:
                    self.Text_ChatType.SetText("【世界】");
                    break;
                case (int)ChatRoomType.Alliance:
                    self.Text_ChatType.SetText("【联盟】");
                    break;
                case (int)ChatRoomType.Private:
                    self.Text_ChatType.SetText("【私聊】");
                    break;
            }

            self.Text_Content.SetTextFormat("{0}:{1}", chat.Name, chat.Content);
        }
    }
}