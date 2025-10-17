using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIItemTip_EquipmentComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public long ItemId;
        
        public GameObject GameObject { get; set; }
        public TMP_Text Text_ItemName;
        public TMP_Text Text_ItemEquipmentType;
        public TMP_Text Text_Lv;
        public Button Button_Sell;
        public Button Button_Wear;
        public Button Button_TakeOff;
    }
}