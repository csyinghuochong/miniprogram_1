using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UICommonItem))]
    [FriendOf(typeof(UICommonItem))]
    public static partial class UICommonItemSystem
    {
        [EntitySystem]
        private static void Awake(this UICommonItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>();
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Text_ItemNum = rc.Get<GameObject>("Text_ItemNum").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();

            self.Button_Click.onClick.AddListener(() => { self.OnClick().Coroutine(); });
        }

        private static async ETTask OnClick(this UICommonItem self)
        {
            if (self.Parent is UIBagComponent)
            {
                UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
                if (uI != null)
                {
                    uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData() { ItemId = self.ItemId });
                }
            }

            if (self.Parent is UIHeroComponent)
            {
                UIHeroComponent uiHeroComponent = self.Parent as UIHeroComponent;
                UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
                if (uI != null)
                {
                    uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                    {
                        ItemId = self.ItemId,
                        ItemTipOpType = ItemTipOpType.UIHero_Wear,
                        HeroId = uiHeroComponent.CurrentHeroId
                    });
                }
            }
        }

        public static async ETTask UpdateInfo(this UICommonItem self, Item item)
        {
            self.ItemId = item.Id;
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

            self.Text_ItemNum.SetTextFormat("{0}", item.Num);

            string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, itemConfig.ItemQuality.ToString());
            self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }
    }
}