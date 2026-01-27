using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIServerOpenRewardComponent : Entity, IAwake, IDestroy
    {
        public int CurrentPage { get; set; } = 0;
        public List<UIRewardItem> UIRewardItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Type_Lv;
        public Button Button_Type_CE;
        public Transform Content_UIRewardItem;
        public GameObject UIRewardItem;
    }
}