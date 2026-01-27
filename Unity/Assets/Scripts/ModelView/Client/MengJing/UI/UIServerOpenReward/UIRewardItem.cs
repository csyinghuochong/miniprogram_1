using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIRewardItem : Entity, IAwake<GameObject>
    {
        public GameObject GameObject { get; set; }

        public int RewardId;
        public List<UICommonItem> UICommonItemList { get; set; } = new();

        public TMP_Text Text_Required;
        public TMP_Text Text_Progress;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public GameObject GameObject_NotCompleted;
        public GameObject GameObject_Received;
        public Button Button_GetReward;
    }
}