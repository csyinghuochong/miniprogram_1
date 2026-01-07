using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIArchiveRewardComponent : Entity, IAwake, IDestroy
    {
        public List<UIArchiveRewardItem> UIArchiveRewardItemList { get; set; } = new();

        public Button Button_Close;
        public Transform Content_UIArchiveRewardItem;
        public GameObject UIArchiveRewardItem;
        public TMP_Text Text_CurrentPoints;
    }
}