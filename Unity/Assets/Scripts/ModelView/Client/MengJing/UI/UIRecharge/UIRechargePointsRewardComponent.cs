using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIRechargePointsRewardComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
        
        public List<UIRechargePointsRewardItem> UIRechargePointsRewardItemList { get; set; } = new();

        public Button Button_Close;
        public Transform Content_UIRechargePointsRewardItem;
        public GameObject UIRechargePointsRewardItem;
        public TMP_Text Text_VipLv;
        public Image Image_PointsProgress;
        public TMP_Text Text_Points;
    }
}