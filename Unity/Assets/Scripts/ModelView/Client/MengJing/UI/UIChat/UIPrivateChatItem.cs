using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIPrivateChatItem : Entity, IAwake<GameObject>
    {

        public GameObject GameObject;

        public Image Image_SpeakerHead;
        public Button Button_OnSpeakerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_Content;
    }
}