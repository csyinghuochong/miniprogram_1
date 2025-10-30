using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UICommonItem : Entity, IAwake<GameObject>
    {
        public long ItemId;

        public Action<long> OnItemClick;
        public GameObject GameObject { get; set; }

        public Image Image_ItemQuality;
        public Image Image_On;
        public Image Image_ItemIcon;
        public TMP_Text Text_ItemNum;
        public Button Button_Click;
        public Image Image_Selected { get; set; }
    }
}