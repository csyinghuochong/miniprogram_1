using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIEquipmentItem))]
    [FriendOf(typeof(UIEquipmentItem))]
    public static partial class UIEquipmentItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIEquipmentItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>();
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();

            self.Button_Click.onClick.AddListener(() => { self.OnClick(); });
        }

        [EntitySystem]
        private static void Destroy(this UIEquipmentItem self)
        {
        }

        private static void OnClick(this UIEquipmentItem self)
        {
            if (self.Parent is UIHeroComponent)
            {
                self.GetParent<UIHeroComponent>().ShowItemList();
            }
        }

        public static async ETTask UpdateInfo(this UIEquipmentItem self, Hero hero)
        {
            self.HeroId = hero.Id;

            if (!hero.Equipments.ContainsKey((int)self.EquipSlotType) || hero.Equipments[(int)self.EquipSlotType] == 0)
            {
                self.Image_ItemQuality.overrideSprite = null;
                self.Image_ItemIcon.overrideSprite = null;
            }
            else
            {
                Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(hero.Equipments[(int)self.EquipSlotType]);

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                string qualityPath = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ZString.Format("quality{0}", itemConfig.ItemQuality));
                self.Image_ItemQuality.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(qualityPath);

                string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
                self.Image_ItemIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            }
        }
    }
}