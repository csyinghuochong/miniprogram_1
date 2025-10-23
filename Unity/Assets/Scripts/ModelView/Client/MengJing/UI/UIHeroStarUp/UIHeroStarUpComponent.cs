using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroStarUpComponent : Entity, IAwake, IDestroy
    {
        public long HeroId { get; set; }
        public long ItemId;
        public List<UICommonItem> UICommonItemList { get; set; } = new();

        public Button Button_Close;
        public TMP_Text Text_HeroName;
        public TMP_Text Text_HeroLv;
        public Transform Transform_HeroStar;
        public Slider Slider_HeroHunShi;
        public TMP_Text Text_HeroHunShi;
        public Transform Content_UICommonItem;
        public TMP_Text Text_Tip;
        public Button Button_Use_10;
        public Button Button_Use_1;
        public GameObject UICommonItem;
    }
}