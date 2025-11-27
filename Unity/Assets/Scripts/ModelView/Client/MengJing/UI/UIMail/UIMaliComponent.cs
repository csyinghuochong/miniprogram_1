using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIMailComponent: Entity, IAwake,IDestroy
    {
        public List<UIMailItem> UIMailItemList { get; set; } = new();
        
        public Button Button_DeleteAll;
        public Button Button_GetAll;
        public Button Button_Close;
        public Transform Content_UIMailItem;
        public GameObject UIMailItem;
    }
}