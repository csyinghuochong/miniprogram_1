using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIHeroInfoComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }

        public long CurrentHeroId { get; set; }

        public List<UISkillItem> UISkillItemList { get; set; } = new();
        public List<UICommonItem> UICommonItemList { get; set; } = new();
        public List<UITeamItem> UITeamItemList { get; set; } = new();
        

        public GameObject UIHeroInfo_1;
        public Transform Spine_HeroModel;
        public TMP_Text Text_HeroName;
        public TMP_Text Text_HeroCP;
        public TMP_Text Text_HeroLv;
        public Slider Slider_HeroExp;
        public TMP_Text Text_HeroExp;
        public UIEquipmentItem UIEquipmentItem_1 { get; set; }
        public UIEquipmentItem UIEquipmentItem_2 { get; set; }
        public UIEquipmentItem UIEquipmentItem_3 { get; set; }
        public UIEquipmentItem UIEquipmentItem_4 { get; set; }
        public UIEquipmentItem UIEquipmentItem_5 { get; set; }
        public UIEquipmentItem UIEquipmentItem_6 { get; set; }
        public Transform Transform_HeroStar;
        public GameObject UIHeroInfo_2;
        public Transform Content_UIBaseAttributeItem;
        public GameObject UIBaseAttributeItem;
        public Transform Content_UIOtherAttributeItem;
        public GameObject UIOtherAttributeItem;
        public Transform Content_UISkillItem;
        public GameObject UISkillItem;
        public Button Button_XiangXi;
        public Button Button_ShengXing;
        public Button Button_ShengJi;
        public GameObject ScrollView_ItemList;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public Transform Content_UITeamItem;
        public GameObject UITeamItem;
    }
}