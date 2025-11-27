using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIGetRewardComponent : Entity, IAwake
    {
        public Button Button_Close;
        
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
    }
}