using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIMonthSignInComponent : Entity, IAwake, IDestroy
    {
        public List<UICommonItem> UICommonItemList { get; set; } = new();

        public Button Button_Close;
        public TMP_Text Text_SignedInDays;
        public Image Image_SignedInDays;
        public Button Button_SignedInSeven;
        public Button Button_SignedInFourteen;
        public Button Button_SignedInTwentyOne;
        public Button Button_SignedInThirty;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
        public Button Button_SignIn;
    }
}