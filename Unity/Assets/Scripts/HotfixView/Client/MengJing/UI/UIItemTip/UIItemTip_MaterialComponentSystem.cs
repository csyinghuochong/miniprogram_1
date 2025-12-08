using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTip_MaterialComponent))]
    [FriendOf(typeof(UIItemTip_MaterialComponent))]
    public static partial class UIItemTip_MaterialComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTip_MaterialComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_Lv = rc.Get<GameObject>("Text_Lv").GetComponent<TMP_Text>();
            self.Text_ItemDescription = rc.Get<GameObject>("Text_ItemDescription").GetComponent<TMP_Text>();
            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>();
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();
            self.Button_Save = rc.Get<GameObject>("Button_Save").GetComponent<Button>();
            self.Button_Take = rc.Get<GameObject>("Button_Take").GetComponent<Button>();

            self.Button_Sell.gameObject.SetActive(false);
            self.Button_Save.gameObject.SetActive(false);
            self.Button_Take.gameObject.SetActive(false);

            self.Button_Sell.AddListener(() => { self.OnButton_Sell().Coroutine(); });
            self.Button_Save.AddListener(() => { self.OnButton_Save(); });
            self.Button_Take.AddListener(() => { self.OnButton_Take(); });
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_MaterialComponent self)
        {
        }

        private static async ETTask OnButton_Sell(this UIItemTip_MaterialComponent self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemSellTip);
            UIItemSellTipComponent uiItemSellTipComponent = ui.GetComponent<UIItemSellTipComponent>();
            uiItemSellTipComponent.InitUI(self.UIItemTipData);

            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }

        public static async ETTask UpdateInfo(this UIItemTip_MaterialComponent self, UIItemTipData uiItemTipData)
        {
            self.UIItemTipData = uiItemTipData;

            ItemConfig itemConfig = null;
            if (uiItemTipData.Item != null)
            {
                itemConfig = ItemConfigCategory.Instance.Get(uiItemTipData.Item.ConfigId);
            }
            else
            {
                itemConfig = ItemConfigCategory.Instance.Get(uiItemTipData.ItemConfigId);
            }

            string color = itemConfig.ItemQuality switch
            {
                1 => "#0e832a",
                2 => "#2e69c4",
                3 => "#d6bb10",
                4 => "#be5e10",
                5 => "#e200af",
                6 => "#d01a06",
                _ => "#ffffff"
            };

            Color nowColor;
            ColorUtility.TryParseHtmlString(color, out nowColor);

            self.Text_ItemName.SetText(itemConfig.ItemName);
            self.Text_ItemName.color = nowColor;
            self.Text_ItemDescription.text = itemConfig.ItemDescription;
            self.Text_Lv.SetTextFormat("{0}级", itemConfig.UseLv);

            string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", itemConfig.ItemQuality));
            self.Image_ItemQuality.overrideSprite =
                    await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            if (uiItemTipData.UIItemTipOpType == UIItemTipOpType.Bag2Warehouse)
            {
                self.Button_Save.gameObject.SetActive(true);
            }

            if (self.UIItemTipData.UIItemTipOpType == UIItemTipOpType.Warehouse2Bag)
            {
                self.Button_Take.gameObject.SetActive(true);
            }

            if (uiItemTipData.UIItemTipOpType == UIItemTipOpType.OnRoleBag)
            {
                self.Button_Sell.gameObject.SetActive(true);
            }
        }

        private static void OnButton_Save(this UIItemTip_MaterialComponent self)
        {
            if (self.UIItemTipData.UIItemTipOpType == UIItemTipOpType.Bag2Warehouse)
            {
                ClientInventoryHelper.MoveItem(self.Root(), self.UIItemTipData.Item.Id, InventoryContainerType.Warehouse).Coroutine();
            }

            self.OnClose();
        }

        private static void OnButton_Take(this UIItemTip_MaterialComponent self)
        {
            if (self.UIItemTipData.UIItemTipOpType == UIItemTipOpType.Warehouse2Bag)
            {
                ClientInventoryHelper.MoveItem(self.Root(), self.UIItemTipData.Item.Id, InventoryContainerType.Bag).Coroutine();
            }

            self.OnClose();
        }

        private static void OnClose(this UIItemTip_MaterialComponent self)
        {
            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }
    }
}