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
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();
            self.Button_Wear = rc.Get<GameObject>("Button_Wear").GetComponent<Button>();
            self.Button_TakeOff = rc.Get<GameObject>("Button_TakeOff").GetComponent<Button>();

            self.Button_Sell.gameObject.SetActive(false);
            self.Button_Wear.gameObject.SetActive(false);
            self.Button_TakeOff.gameObject.SetActive(false);

            self.Button_Sell.AddListener(() => { self.OnButton_Sell().Coroutine(); });
            self.Button_Wear.AddListener(self.OnButton_Wear);
            self.Button_TakeOff.AddListener(self.OnButton_TakeOff);
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_EquipmentComponent self)
        {
        }

        public static async ETTask UpdateInfo(this UIItemTip_EquipmentComponent self, UIItemTipData uiItemTipData)
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
            self.Text_Lv.SetTextFormat("{0}级", itemConfig.UseLv);
            
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            if (uiItemTipData.UIItemTipOpType == UIItemTipOpType.UIHero_Wear)
            {
                self.Button_Sell.gameObject.SetActive(true);
                self.Button_Wear.gameObject.SetActive(true);
            }

            if (uiItemTipData.UIItemTipOpType == UIItemTipOpType.UIHero_TakeOff)
            {
                self.Button_TakeOff.gameObject.SetActive(true);
            }
        }

        private static async ETTask OnButton_Sell(this UIItemTip_EquipmentComponent self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemSellTip);
            UIItemSellTipComponent uiItemSellTipComponent = ui.GetComponent<UIItemSellTipComponent>();
            uiItemSellTipComponent.InitUI(self.UIItemTipData);

            self.OnClose();
        }

        private static void OnButton_Wear(this UIItemTip_EquipmentComponent self)
        {
            ClientHeroHelper.SetHeroEquipment(self.Root(), 0, self.UIItemTipData.HeroId, self.UIItemTipData.ItemId).Coroutine();
            self.OnClose();
        }

        private static void OnButton_TakeOff(this UIItemTip_EquipmentComponent self)
        {
            ClientHeroHelper.SetHeroEquipment(self.Root(), 1, self.UIItemTipData.HeroId, self.UIItemTipData.ItemId).Coroutine();
            self.OnClose();
        }

        private static void OnClose(this UIItemTip_EquipmentComponent self)
        {
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }
    }
}