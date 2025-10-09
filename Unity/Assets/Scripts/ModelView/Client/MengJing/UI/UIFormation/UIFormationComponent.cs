using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIFormationComponent : Entity, IAwake, IDestroy
    {
        public int ShowHeroType;
        public List<UIFormationHeroItem> UIFormationHeroItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Plan_1;
        public Button Button_Plan_2;
        public UIFormationSlotItem UIFormationSlotItem_1 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_2 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_3 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_4 { get; set; }
        public UIFormationSlotItem UIFormationSlotItem_5 { get; set; }
        public Transform Content_UIFormationHeroItem;
        public GameObject UIFormationHeroItem;
    }
}