using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIPlayerInfoComponent : Entity, IAwake, IDestroy
    {
        public long UnitId;
        public List<UIFormationSlotItem> UIFormationSlotItemList { get; set; } = new();

        public Button Button_Close;
        public Image Image_PlayerHead;
        public Button Button_OnPlayerHead;
        public TMP_Text Text_PlayerName;
        public TMP_Text Text_PlayerCE;
        public TMP_Text Text_PlayerLianMeng;
        public Button Button_AddFriend;
        public Button Button_DeleteFriend;
        public Button Button_Report;
        public Button Button_Black;
        public Button Button_UnBlack;
        public Transform Transform_UIFormationSlotItemList;
    }
}