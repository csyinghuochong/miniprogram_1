using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIItemTip_MaterialComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public UIItemTipData UIItemTipData;

        public GameObject GameObject { get; set; }

        public TMP_Text Text_ItemName;
        public TMP_Text Text_ItemDescription;
        public Button Button_Sell;
    }
}