
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroRecycleItem))]
    [FriendOf(typeof(UIHeroRecycleItem))]
    public static partial class UIHeroRecycleItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroRecycleItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_HeroQuality = rc.Get<GameObject>("Image_HeroQuality").GetComponent<Image>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Image_Selected = rc.Get<GameObject>("Image_Selected").GetComponent<Image>();
            self.Image_Selected.gameObject.SetActive(false);
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
        }

        public static async ETTask UpdateInfo(this UIHeroRecycleItem self, Hero hero)
        {
            if (hero == null)
            {
                self.Image_HeroQuality.gameObject.SetActive(false);
                self.Image_HeroIcon.gameObject.SetActive(false);
                return;
            }
            
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            
            self.Image_HeroQuality.gameObject.SetActive(true);
            self.Image_HeroIcon.gameObject.SetActive(true);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }
    }
}