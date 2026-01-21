using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIRecycleComponent : Entity, IAwake, IDestroy
    {
        public int CurrentPage { get; set; } = 0;

        public List<EntityRef<Item>> SelectItemList = new();
        public List<EntityRef<Hero>> SelectHeroList = new();

        public List<UICommonItem> UICommonItemList { get; set; } = new();
        public List<UICommonItem> UILookRewardList { get; set; } = new();
        public List<UIHeroRecycleItem> UIHeroRecycleItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Type_Bag;
        public Button Button_Type_Hero;
        public GameObject GameObject_Bag;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public GameObject GameObject_Hero;
        public Transform Content_UIHeroRecycleItem;
        public GameObject UIHeroRecycleItem;
        public Transform Content_LookReward;
        public Button Button_Recycle;
    }
}