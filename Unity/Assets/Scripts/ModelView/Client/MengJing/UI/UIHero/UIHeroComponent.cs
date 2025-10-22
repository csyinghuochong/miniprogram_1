using System.Collections.Generic;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIHeroComponent : Entity, IAwake, IDestroy
    {
        public long CurrentHeroId { get; set; }
        public List<UICommonItem> UICommonItemList { get; set; } = new();
        public List<UITeamItem> UITeamItemList { get; set; } = new();

        public Button Button_Close;

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
        public GameObject UIHeroInfo_2;
        public Transform Content_UIBaseStatItem;
        public GameObject UIBaseStatItem;
        public Transform Content_UIOtherStatItem;
        public GameObject UIOtherStatItem;
        public GameObject ScrollView_ItemList;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public Transform Content_UITeamItem;
        public GameObject UITeamItem;
        public Button Button_Hero;
        public Button Button_Formation;
    }
}