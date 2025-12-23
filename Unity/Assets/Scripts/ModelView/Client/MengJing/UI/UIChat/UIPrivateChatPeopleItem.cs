using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIPrivateChatPeopleItem : Entity, IAwake<GameObject>
    {

        public GameObject GameObject;

        public Image Image_SpeakerHead;
        public TMP_Text Text_PlayerName;
        public Button Button_EnterChat;
    }
}