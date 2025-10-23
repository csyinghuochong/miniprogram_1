using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroLvUpComponent : Entity, IAwake, IDestroy
    {
        public long HeroId;
        public long ItemId;

        public Button Button_Close;
        public TMP_Text Text_HeroName;
        public TMP_Text Text_HeroLv;
        public Slider Slider_HeroExp;
        public TMP_Text Text_HeroExp;
        public Transform Content_UICommonItem;
        public TMP_Text Text_Tip;
        public Button Button_Use_10;
        public Button Button_Use_1;
        public GameObject UICommonItem;
    }
}