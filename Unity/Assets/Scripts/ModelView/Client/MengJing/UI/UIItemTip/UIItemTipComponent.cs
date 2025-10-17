using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public enum UIItemTipOpType
    {
        
    }
    
    [ComponentOf(typeof(UI))]
    public class UIItemTipComponent : Entity, IAwake, IDestroy
    {
        public Button Button_Close;
        public UIItemTip_NormalComponent UIItemTip_NormalComponent { get; set; }
        public UIItemTip_EquipmentComponent UIItemTip_EquipmentComponent { get; set; }
    }
}