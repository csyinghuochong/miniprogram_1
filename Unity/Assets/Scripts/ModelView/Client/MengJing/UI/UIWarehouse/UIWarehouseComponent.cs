using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIWarehouseComponent : Entity, IAwake, IDestroy
    {
        public List<UICommonItem> UIWarehouseItemList { get; set; } = new();
        public List<UICommonItem> UIBagItemList { get; set; } = new();
        
        public Button Button_Close;
        public GameObject UICommonItem;
        public Transform Content_WarehouseItem;
        public Transform Content_BagItem;
    }
}