using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroListComponent : Entity, IAwake, IDestroy
    {
        public List<UIHeroItem> UIHeroItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Type_All;
        public Button Button_Type_Warrior;
        public Button Button_Type_Mage;
        public Button Button_Type_Archer;
        public Transform Content_UIHeroItem;
        public GameObject UIHeroItem;
    }
}