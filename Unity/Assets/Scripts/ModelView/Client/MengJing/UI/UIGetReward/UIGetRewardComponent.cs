using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIGetRewardComponent : Entity, IAwake, IDestroy
    {
        public List<UICommonItem> UIRewardItemList { get; set; } = new();

        public Button Button_Close;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
    }
}