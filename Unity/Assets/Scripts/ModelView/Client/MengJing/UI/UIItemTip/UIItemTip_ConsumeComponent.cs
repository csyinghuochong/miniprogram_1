using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIItemTip_ConsumeComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public long ItemId;

        public GameObject GameObject { get; set; }

        public TMP_Text Text_ItemName;
        public TMP_Text Text_ItemDescription;
        public Button Button_Sell;
        public Button Button_Use;
    }
}