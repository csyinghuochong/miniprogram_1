using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [ComponentOf(typeof(UI))]
    public class UIMainCityMapComponent : Entity, IAwake, IDestroy
    {
        
        public List<UIMainCityMapNPCButton> UIMainCityMapNPCButtonList { get; set; } = new();

        public Button Button_Close;
        public Transform Content_UIMainCityMapNPCButton;
        public GameObject UIMainCityMapNPCButton;
    }
}