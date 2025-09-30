using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroItem))]
    [FriendOf(typeof(UIHeroItem))]
    public static partial class UIHeroItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
        }

        public static async ETTask UpdateInfo(this UIHeroItem self, Hero hero)
        {
            self.HeroId = hero.Id;
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            self.Text_HeroName.text = heroConfig.HeroName;
        }
    }
}