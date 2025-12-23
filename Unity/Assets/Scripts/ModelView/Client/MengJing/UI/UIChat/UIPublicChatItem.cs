using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIPublicChatItem : Entity, IAwake<GameObject>
    {
        private EntityRef<Chat> chat;
        public Chat Chat { get => this.chat; set => this.chat = value; }

        public GameObject GameObject;

        public Image Image_SpeakerHead;
        public Button Button_OnSpeakerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_Content;
    }
}