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
        public List<UIFormationSlotItem> UIFormationSlotItemList { get; set; } = new();

        public TMP_Text Text_FormationCount;
        public TMP_Text Text_TotalCP;
        public Transform Transform_UIFormationSlotItemList;
        public Button Button_Type_All;
        public Button Button_Type_Melee;
        public Button Button_Type_Ranged;
        public Transform Content_UIFormationHeroItem;
        public GameObject UIFormationHeroItem;
    }
}