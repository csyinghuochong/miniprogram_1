using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET.Client
{
    [ChildOf]
    public class UICommonItem : Entity, IAwake<GameObject>, IDestroy
    {
        private EntityRef<Item> item;
        public Item Item { get => item; set => item = value; }

        public int ItemConfigId;

        public Action<Item> OnItemClick { get; set; }
        public Action OnLongPressed;
        public Action OnItemPointerUp;
        public bool IsDrag;
        public bool IsPressing;
        public long PressedTime;
        public long PressedTriggerTime = 1000; // 长按触发时间

        public GameObject GameObject { get; set; }

        public GameObject ItemGO { get; set; }
        public Image Image_ItemNull { get; set; }
        public Image Image_ItemQuality;
        public Image Image_On;
        public Image Image_ItemIcon;
        public TMP_Text Text_ItemNum;
        public Button Button_Click { get; set; }
        public Image Image_Pressed { get; set; }
        public EventTrigger EventTrigger_Click { get; set; }
        public Image Image_Selected { get; set; }
        public Image Image_Equipped { get; set; }
    }
}