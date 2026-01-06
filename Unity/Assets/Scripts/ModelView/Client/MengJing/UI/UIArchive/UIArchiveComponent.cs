using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIArchiveComponent : Entity, IAwake,IDestroy
    {
        public int CurrentPage { get; set; } = 0;

        public List<UIArchiveHeroItem> UIArchiveHeroItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Reward;
        public Button Button_Type_Hero;
        public Transform Content_UIArchiveHeroItem;
        public GameObject UIArchiveHeroItem;
        public TMP_Text Text_CollectProgress;
    }
}