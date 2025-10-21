using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTipComponent))]
    [FriendOf(typeof(UIItemTipComponent))]
    public static partial class UIItemTipComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTipComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.UIItemTip_ConsumeComponent = self.AddComponent<UIItemTip_ConsumeComponent, GameObject>(rc.Get<GameObject>("UIItemTip_Consume"));
            self.UIItemTip_MaterialComponent = self.AddComponent<UIItemTip_MaterialComponent, GameObject>(rc.Get<GameObject>("UIItemTip_Material"));
            self.UIItemTip_EquipmentComponent = self.AddComponent<UIItemTip_EquipmentComponent, GameObject>(rc.Get<GameObject>("UIItemTip_Equipment"));

            self.UIItemTip_ConsumeComponent.GameObject.SetActive(false);
            self.UIItemTip_MaterialComponent.GameObject.SetActive(false);
            self.UIItemTip_EquipmentComponent.GameObject.SetActive(false);
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip); });
        }

        [EntitySystem]
        private static void Destroy(this UIItemTipComponent self)
        {
        }

        public static void UpdateInfo(this UIItemTipComponent self, UIItemTipData uiItemTipData)
        {
            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(uiItemTipData.ItemId);
            if (item != null)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
                if (itemConfig.ItemType == (int)ItemType.Consume)
                {
                    self.UIItemTip_ConsumeComponent.GameObject.SetActive(true);
                    self.UIItemTip_ConsumeComponent.UpdateInfo(uiItemTipData);
                }

                if (itemConfig.ItemType == (int)ItemType.Material)
                {
                    self.UIItemTip_MaterialComponent.GameObject.SetActive(true);
                    self.UIItemTip_MaterialComponent.UpdateInfo(uiItemTipData);
                }

                if (itemConfig.ItemType == (int)ItemType.Equipment)
                {
                    self.UIItemTip_EquipmentComponent.GameObject.SetActive(true);
                    self.UIItemTip_EquipmentComponent.UpdateInfo(uiItemTipData);
                }
            }
        }
    }
}