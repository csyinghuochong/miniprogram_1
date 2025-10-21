using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTip_EquipmentComponent))]
    [FriendOf(typeof(UIItemTip_EquipmentComponent))]
    public static partial class UIItemTip_EquipmentComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTip_EquipmentComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_ItemEquipmentType = rc.Get<GameObject>("Text_ItemEquipmentType").GetComponent<TMP_Text>();
            self.Text_Lv = rc.Get<GameObject>("Text_Lv").GetComponent<TMP_Text>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();
            self.Button_Wear = rc.Get<GameObject>("Button_Wear").GetComponent<Button>();
            self.Button_TakeOff = rc.Get<GameObject>("Button_TakeOff").GetComponent<Button>();

            self.Button_Sell.gameObject.SetActive(false);
            self.Button_Wear.gameObject.SetActive(false);
            self.Button_TakeOff.gameObject.SetActive(false);
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_EquipmentComponent self)
        {
        }

        public static void UpdateInfo(this UIItemTip_EquipmentComponent self, UIItemTipData uiItemTipData)
        {
            self.UIItemTipData = uiItemTipData;

            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(uiItemTipData.ItemId);
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
            EquipConfig equipConfig = EquipConfigCategory.Instance.Get(itemConfig.ItemEquipID);

            string type = itemConfig.ItemSubType switch
            {
                (int)ItemEquipmentType.Toukui => "头盔",
                (int)ItemEquipmentType.Yifu => "衣服",
                (int)ItemEquipmentType.Kuzi => "裤子",
                (int)ItemEquipmentType.Xiezi => "鞋子",
                (int)ItemEquipmentType.Xianglian => "项链",
                (int)ItemEquipmentType.Wuqi => "武器",
                _ => ""
            };

            self.Text_ItemName.SetText(itemConfig.ItemName);
            self.Text_ItemEquipmentType.SetText(type);
            self.Text_Lv.SetTextFormat("{0}级", 0);

            if (uiItemTipData.UIItemTipOpType == UIItemTipOpType.UIHero_Wear)
            {
                self.Button_Sell.gameObject.SetActive(true);
                self.Button_Wear.gameObject.SetActive(true);

                self.Button_Wear.AddListener(self.OnButton_Wear);
            }
        }

        private static void OnButton_Wear(this UIItemTip_EquipmentComponent self)
        {
            HeroHelper.SetHeroEquipment(self.Root(), 0, self.UIItemTipData.HeroId, self.UIItemTipData.ItemId).Coroutine();
            self.OnClose();
        }

        private static void OnClose(this UIItemTip_EquipmentComponent self)
        {
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }
    }
}