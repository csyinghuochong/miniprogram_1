using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIHeroDetailsComponent : Entity, IAwake, IDestroy
    {
        public int CurrentHeroConfigId { get; set; }

        public List<UISkillItem> UISkillItemList { get; set; } = new();

        public Button Button_Close;
        public Transform Spine_HeroModel;
        public TMP_Text Text_HeroCP;
        public TMP_Text Text_HeroQuality;
        public TMP_Text Text_HeroName;
        public Transform Transform_HeroStar;
        public TMP_Text Text_HeroType;
        public TMP_Text Text_HeroLv;
        public Transform Content_UIBaseAttributeItem;
        public GameObject UIBaseAttributeItem;
        public Transform Content_UISkillItem;
        public GameObject UISkillItem;
    }
}