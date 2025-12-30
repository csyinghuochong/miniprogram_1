using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIRankComponent : Entity, IAwake
    {
        public int CurrentPage { get; set; } = 0;

        public List<UIRankCEItem> UIRankCEItemList { get; set; } = new();
        public List<UIRankLianMengItem> UIRankLianMengItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Type_CE;
        public Button Button_Type_LianMeng;
        public GameObject Scroll_UIRankCEItem;
        public Transform Content_UIRankCEItem;
        public GameObject UIRankCEItem;
        public GameObject Scroll_UIRankLianMengItem;
        public Transform Content_UIRankLianMengItem;
        public GameObject UIRankLianMengItem;
        public Image Image_SelfHead;
        public Button Button_OnSelfHead;
        public TMP_Text Text_SelfName;
        public TMP_Text Text_SelfCE;
        public TMP_Text Text_SelfSort;
    }
}