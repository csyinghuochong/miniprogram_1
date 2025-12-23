using TMPro;
using UnityEngine;

namespace ET.Client
{
    [ChildOf]
    public class UIMainChatItem : Entity, IAwake<GameObject>
    {
        private EntityRef<ChatEntry> chatEntry;
        public ChatEntry ChatEntry { get => this.chatEntry; set => this.chatEntry = value; }
        public GameObject GameObject { get; set; }

        public TMP_Text Text_ChatType;
        public TMP_Text Text_Content;
    }
}