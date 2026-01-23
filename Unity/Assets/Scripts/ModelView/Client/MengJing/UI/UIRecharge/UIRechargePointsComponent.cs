using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf]
    public class UIRechargePointsComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject GameObject { get; set; }
        
        public List<UIRechargePointsItem> UIRechargePointsItemList { get; set; } = new();

        public Button Button_Close;
        public Transform Content_UIRechargePointsItem;
        public GameObject UIRechargePointsItem;
        public TMP_Text Text_VipLv;
        public Image Image_PointsProgress;
        public TMP_Text Text_Points;
    }
}