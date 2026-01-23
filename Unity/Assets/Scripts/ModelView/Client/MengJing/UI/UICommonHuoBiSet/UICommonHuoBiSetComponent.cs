using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf]
    public class UICommonHuoBiSetComponent : Entity, IAwake<GameObject>, IDestroy
    {
        [StaticField]
        public static List<UICommonHuoBiSetComponent> InstanceList = new();

        public GameObject GameObject;
        public GameObject LotteryTicket { get; set; }
        public TMP_Text Text_Type_LotteryTicket;
        public GameObject Gold { get; set; }
        public TMP_Text Text_Type_Gold;
        public Button Button_AddGold;
        public GameObject Diamond { get; set; }
        public TMP_Text Text_Type_Diamond;
        public Button Button_AddDiamond;
    }
}