using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIBlackItem : Entity, IAwake<GameObject>
    {
        private EntityRef<FriendData> friendData;
        public FriendData FriendData { get => this.friendData; set => this.friendData = value; }

        public GameObject GameObject { get; set; }

        public Image Image_PlayerHead;
        public Button Button_OnPlayerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_PlayerLv;
        public TMP_Text Text_PlayerCE;
        public TMP_Text Text_PlayerStatus;
        public Button Button_CancelBlack;
    }
}