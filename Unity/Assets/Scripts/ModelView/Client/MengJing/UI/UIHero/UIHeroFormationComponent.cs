using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIHeroFormationComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }

        public int ShowHeroType;
        public List<UIFormationHeroItem> UIFormationHeroItemList { get; set; } = new();

        public TMP_Text Text_TotalCP;
        public UIFormationSlotItem UIFormationSlotItem_1 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_2 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_3 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_4 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_5 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_6 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_7 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_8 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_9 { get; set; }
        
        public Button Button_Type_All;
        public Button Button_Type_Melee;
        public Button Button_Type_Ranged;
        public Transform Content_UIFormationHeroItem;
        public GameObject UIFormationHeroItem;
    }
}