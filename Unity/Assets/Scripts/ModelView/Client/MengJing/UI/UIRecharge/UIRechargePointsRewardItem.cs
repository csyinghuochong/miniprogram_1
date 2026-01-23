using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIRechargePointsRewardItem : Entity, IAwake<GameObject>, IDestroy
    {

        public List<UICommonItem> UIRewardItemList { get; set; } = new();
        public GameObject GameObject { get; set; }

        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public TMP_Text Text_RequiredPoints;
        public GameObject GameObject_Received;
        public Button Button_GetReward;

    }
}