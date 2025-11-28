using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIItemTip_EquipmentComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public UIItemTipData UIItemTipData;
        
        public GameObject GameObject { get; set; }
        public TMP_Text Text_ItemName;
        public TMP_Text Text_ItemEquipmentType;
        public TMP_Text Text_Lv;
        public Image Image_CombatPowerChange;
        public TMP_Text Text_CombatPowerChange;
        public Image Image_CombatPowerReduction;
        public Image Image_CombatPowerIncrease;
        public Transform BaseAttributeList;
        public GameObject UIAttributeItem;
        public Image Image_ItemQuality;
        public Image Image_ItemIcon;
        public Button Button_Sell;
        public Button Button_Wear;
        public Button Button_TakeOff;
        public TMP_Text Text_EquipHero;
        public Button Button_Save;
        public Button Button_Take;
    }
}