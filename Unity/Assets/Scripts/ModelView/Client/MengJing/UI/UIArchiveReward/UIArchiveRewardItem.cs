using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIArchiveRewardItem : Entity, IAwake<GameObject>
    {
        public GameObject GameObject { get; set; }

        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public Image Image_PointsProgress;
        public TMP_Text Text_RewardPoints;
        public Button Button_GetReward;
    }
}