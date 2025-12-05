using TMPro;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIStoreRefTipComponent : Entity, IAwake
    {
        public TMP_Text Text_Tip;
        public Button Button_Refresh;
        public Button Button_Close;
    }
}