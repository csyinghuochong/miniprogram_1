using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIItemTip_ConsumeComponent))]
    [FriendOf(typeof(UIItemTip_ConsumeComponent))]
    public static partial class UIItemTip_ConsumeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIItemTip_ConsumeComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_ItemName = rc.Get<GameObject>("Text_ItemName").GetComponent<TMP_Text>();
            self.Text_Lv = rc.Get<GameObject>("Text_Lv").GetComponent<TMP_Text>();
            self.Text_ItemDescription = rc.Get<GameObject>("Text_ItemDescription").GetComponent<TMP_Text>();
            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>();
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Button_Sell = rc.Get<GameObject>("Button_Sell").GetComponent<Button>();
            self.Button_Use = rc.Get<GameObject>("Button_Use").GetComponent<Button>();

            self.Button_Sell.AddListener(() => { self.OnButton_Sell().Coroutine(); });
            self.Button_Use.AddListener(() => { self.OnButton_Use(); });
        }

        [EntitySystem]
        private static void Destroy(this UIItemTip_ConsumeComponent self)
        {
        }

        private static async ETTask OnButton_Sell(this UIItemTip_ConsumeComponent self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemSellTip);
            UIItemSellTipComponent uiItemSellTipComponent = ui.GetComponent<UIItemSellTipComponent>();
            uiItemSellTipComponent.InitUI(self.UIItemTipData);

            self.Root().GetComponent<UIComponent>().Remove(UIType.UIItemTip);
        }

        public static async ETTask UpdateInfo(this UIItemTip_ConsumeComponent self, UIItemTipData uiItemTipData)
        {
            self.UIItemTipData = uiItemTipData;

            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(uiItemTipData.ItemId);
            if (item != null)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
                
                self.Text_ItemName.SetText(itemConfig.ItemName);
                self.Text_ItemDescription.text = itemConfig.ItemDescription;
                self.Text_Lv.SetTextFormat("{0}级", itemConfig.UseLv);
                
                string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", itemConfig.ItemQuality));
                self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);
                
                string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
                self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            }
        }
        
        private static void OnButton_Use(this UIItemTip_ConsumeComponent self)
        { 
            ClientInventoryHelper.UseItem(self.Root(), self.UIItemTipData.ItemId, 1, 0).Coroutine();
        }
    }
}