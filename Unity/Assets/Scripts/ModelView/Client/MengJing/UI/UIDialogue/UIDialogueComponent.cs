
using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIDialogueComponent : Entity, IAwake
    {
        public Button Button_Close;
        public TMP_Text Text_NpcName;
        public TMP_Text Text_Content;
    }
}