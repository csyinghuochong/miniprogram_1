using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIItemTipComponent : Entity, IAwake, IDestroy
    {
        public long ItemId;
        
        public Button Button_Close;
        public TMP_Text Text_ItemName;
        public TMP_Text Text_ItemDescription;
        public Button Button_Use;
    }
}