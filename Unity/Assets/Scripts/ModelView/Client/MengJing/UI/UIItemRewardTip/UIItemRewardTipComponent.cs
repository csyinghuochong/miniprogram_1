using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIItemRewardTipComponent : Entity, IAwake
    {
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
    }
}