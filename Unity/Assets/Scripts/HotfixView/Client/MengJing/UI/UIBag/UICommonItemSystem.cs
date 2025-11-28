using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

            self.Item = rc.Get<GameObject>("Item");
            self.Image_ItemNull = rc.Get<GameObject>("Image_ItemNull").GetComponent<Image>();
            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>();
            self.Image_On = rc.Get<GameObject>("Image_On").GetComponent<Image>();
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Text_ItemNum = rc.Get<GameObject>("Text_ItemNum").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
            self.Image_Pressed = rc.Get<GameObject>("Image_Pressed").GetComponent<Image>();
            self.EventTrigger_Click = rc.Get<GameObject>("EventTrigger_Click").GetComponent<EventTrigger>();
            self.Image_Selected = rc.Get<GameObject>("Image_Selected").GetComponent<Image>();
            self.Image_Equipped = rc.Get<GameObject>("Image_Equipped").GetComponent<Image>();

            self.Image_On.gameObject.SetActive(false);
            self.Image_Equipped.gameObject.SetActive(false);
            self.Image_Selected.gameObject.SetActive(false);

            self.Button_Click.AddListener(self.OnClick);
        }

        private static void OnClick(this UICommonItem self)
        {
            self.OnItemClick?.Invoke(self.ItemId);
        }

        public static async ETTask UpdateInfo(this UICommonItem self, Item item, Action<long> onItemClick = null)
        {
            self.ItemId = item.Id;
            self.OnItemClick = onItemClick;
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

            self.Text_ItemNum.SetText(item.Num);

            string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", itemConfig.ItemQuality));
            self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }

        public static async ETTask UpdateInfo(this UICommonItem self, int itemConfigId, int num)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemConfigId);

            self.Text_ItemNum.SetText(num);

            string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", itemConfig.ItemQuality));
            self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }

        public static void SetSelected(this UICommonItem self, long itemId)
        {
            self.Image_Selected.gameObject.SetActive(self.ItemId == itemId);
        }

        public static void SetImageOn(this UICommonItem self, long itemId)
        {
            self.Image_Selected.gameObject.SetActive(self.ItemId == itemId);
        }
    }
}