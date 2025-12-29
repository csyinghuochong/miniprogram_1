using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIPrivateChatPeopleItem : Entity, IAwake<GameObject>
    {
        private EntityRef<FriendData> friendData;
        public FriendData FriendData { get => this.friendData; set => this.friendData = value; }

        public GameObject GameObject { get; set; }

        public Image Image_SpeakerHead;
        public TMP_Text Text_PlayerName;
        public Button Button_EnterChat;
    }
}