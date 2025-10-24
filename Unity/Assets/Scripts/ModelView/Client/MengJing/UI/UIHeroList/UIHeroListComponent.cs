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

        public TMP_Text Text_HaveHeroCount;
        public Button Button_Close;
        public Button Button_Type_All;
        public Button Button_Type_Melee;
        public Button Button_Type_Ranged;
        public Transform Content_UIHeroItem;
        public GameObject UIHeroItem;
        public Button Button_Hero;
        public Button Button_HeroList;
        public Button Button_Formation;
    }
}