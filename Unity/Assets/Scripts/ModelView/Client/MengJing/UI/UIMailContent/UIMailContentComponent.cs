using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIMailContentComponent : Entity, IAwake, IDestroy
    {
        public long MailId;
        public List<UICommonItem> UICommonItemList { get; set; } = new();

        public TMP_Text Text_Title;
        public TMP_Text Text_From;
        public TMP_Text Text_Time;
        public TMP_Text Text_Content;
        public Button Button_Close;
        public Button Button_Get;
        public Button Button_Delete;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
    }
}