using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIBagComponent : Entity, IAwake, IDestroy
    {
        public int CurrentPage { get; set; } = 0;
        public List<UICommonItem> UICommonItemList { get; set; } = new();

        public Button Button_Close;
        public Button Button_Type_All;
        public Button Button_Type_Consume;
        public Button Button_Type_Material;
        public Button Button_Type_Equipment;
        public Transform Content_UICommonItem;
        public GameObject UICommonItem;
    }
}