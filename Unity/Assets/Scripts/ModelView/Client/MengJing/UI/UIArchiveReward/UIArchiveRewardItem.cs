using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIArchiveRewardItem : Entity, IAwake<GameObject>
    {
        public int RewardId;
        public List<UICommonItem> UICommonItemList { get; set; } = new();

        public GameObject GameObject { get; set; }

        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public Image Image_PointsProgress;
        public TMP_Text Text_RewardPoints;
        public Button Button_GetReward;
        public GameObject GameObject_Received;
    }
}