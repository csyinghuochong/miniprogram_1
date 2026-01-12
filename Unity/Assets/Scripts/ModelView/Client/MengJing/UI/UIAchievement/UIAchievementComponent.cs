using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIAchievementComponent : Entity, IAwake, IDestroy
    {
        public int CurrentPage { get; set; } = 0;
        public int RewardId;

        public List<UIAchievementItem> UIAchievementItemList { get; set; } = new();
        public List<UICommonItem> UICommonItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Type_Hero;
        public Transform Content_UIAchievementItem;
        public GameObject UIAchievementItem;
        public Image Image_CurrentPoints;
        public TMP_Text Text_CurrentPoints;
        public Button Button_GetReward;
        public GameObject GameObject_LookReward;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
    }
}