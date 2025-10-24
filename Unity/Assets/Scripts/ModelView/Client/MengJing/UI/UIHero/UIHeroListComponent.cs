using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIHeroListComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }

        public List<UIHeroItem> UIHeroItemList { get; set; } = new();

        public TMP_Text Text_HaveHeroCount;
        public Button Button_Type_All;
        public Button Button_Type_Melee;
        public Button Button_Type_Ranged;
        public Transform Content_UIHeroItem;
        public GameObject UIHeroItem;
    }
}