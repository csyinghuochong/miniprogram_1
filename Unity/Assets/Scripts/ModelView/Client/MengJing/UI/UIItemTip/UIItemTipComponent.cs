using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    public enum UIItemTipOpType
    {
        None = 0,
        UIHero_Wear,
        UIHero_TakeOff,
        OnWarehouse,
        Bag2Warehouse,
        Warehouse2Bag,
        OnRoleBag,
    }

    public struct UIItemTipData
    {
        private EntityRef<Item> item;
        public Item Item { get => item; set=> item = value; } //存在有实体
        public int ItemConfigId; //不存在实体
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