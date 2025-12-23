using TMPro;
using UnityEngine;

namespace ET.Client
{
    [ChildOf]
    public class UIMainChatItem : Entity, IAwake<GameObject>
    {
        private EntityRef<Chat> chat;
        public Chat Chat { get => this.chat; set => this.chat = value; }
        public GameObject GameObject { get; set; }

        public TMP_Text Text_ChatType;
        public TMP_Text Text_Content;
    }
}