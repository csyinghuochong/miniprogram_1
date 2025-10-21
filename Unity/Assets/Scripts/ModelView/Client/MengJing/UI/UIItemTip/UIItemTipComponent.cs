using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public enum UIItemTipOpType
    {
        None = 0,
        UIHero_Wear
    }

    public struct UIItemTipData
    {
        public long ItemId;
        public UIItemTipOpType UIItemTipOpType;
        public long HeroId;
    }

    [ComponentOf(typeof(UI))]
    public class UIItemTipComponent : Entity, IAwake, IDestroy
    {
        public Button Button_Close;
        public UIItemTip_ConsumeComponent UIItemTip_ConsumeComponent { get; set; }
        public UIItemTip_MaterialComponent UIItemTip_MaterialComponent { get; set; }
        public UIItemTip_EquipmentComponent UIItemTip_EquipmentComponent { get; set; }
    }
}