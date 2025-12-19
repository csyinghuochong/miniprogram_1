using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIChatItem : Entity, IAwake
    {
        public Image Image_SpeakerHead;
        public Button Button_OnSpeakerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_Content;
    }
}