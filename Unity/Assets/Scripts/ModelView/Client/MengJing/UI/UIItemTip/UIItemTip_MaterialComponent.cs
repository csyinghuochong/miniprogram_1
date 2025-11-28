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
        public TMP_Text Text_Lv;
        public TMP_Text Text_ItemDescription;
        public Image Image_ItemQuality;
        public Image Image_ItemIcon;
        public Button Button_Sell;
        public Button Button_Save;
        public Button Button_Take;
    }
}