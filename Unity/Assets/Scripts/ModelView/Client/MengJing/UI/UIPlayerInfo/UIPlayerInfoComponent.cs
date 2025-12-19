using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIPlayerInfoComponent : Entity, IAwake
    {
        public Button Button_Close;
        public Image Image_PlayerHead;
        public Button Button_OnPlayerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_PlayerCE;
        public TMP_Text Text_PlayerLianMeng;
        public Button Button_AddFriend;
        public Button Button_Report;
        public Button Button_Black;

    }
}