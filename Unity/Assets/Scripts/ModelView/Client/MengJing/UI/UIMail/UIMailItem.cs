using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UIMailItem : Entity, IAwake<GameObject>
    {
        public GameObject GameObject { get; set; }
        
        public TMP_Text Text_State;
        public TMP_Text Text_Title;
        public TMP_Text Text_Time;
        public TMP_Text Text_DeleteTime;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public Button Button_OnClick;
    }
}