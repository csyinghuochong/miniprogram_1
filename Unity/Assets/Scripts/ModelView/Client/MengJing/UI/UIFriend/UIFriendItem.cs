using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIFriendItem : Entity, IAwake<GameObject>
    {
        public GameObject GameObject;

        public Image Image_PlayerHead;
        public Button Button_OnPlayerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_PlayerLv;
        public TMP_Text Text_PlayerCE;
        public TMP_Text Text_PlayerStatus;
        public Button Button_Chat;
        public TMP_Text Text_Sort;
    }
}