using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroComponent : Entity, IAwake, IDestroy
    {
        public long CurrentHeroId;
        public List<UITeamItem> UITeamItemList { get; set; } = new();

        public Button Button_Close;
        public GameObject UIHeroInfo;
        public TMP_Text Text_HeroName;
        public TMP_Text Text_HeroCP;
        public Image Image_HeroIcon;
        public TMP_Text Text_HeroLv;
        public Slider Slider_HeroExp;
        public TMP_Text Text_HeroExp;
        public Transform Content_UITeamItem;
        public GameObject UITeamItem;
        public Button Button_Hero;
        public Button Button_Formation;
    }
}