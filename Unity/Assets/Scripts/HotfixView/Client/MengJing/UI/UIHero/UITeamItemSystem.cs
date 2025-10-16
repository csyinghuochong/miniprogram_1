using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UITeamItem))]
    [FriendOf(typeof(UITeamItem))]
    public static partial class UITeamItemSystem
    {
        [EntitySystem]
        private static void Awake(this UITeamItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_Border = rc.Get<GameObject>("Image_Border").GetComponent<Image>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();

            self.Button_Click.onClick.AddListener(() => { self.GetParent<UIHeroComponent>().SelectHero(self.HeroId); });
        }

        public static async ETTask UpdateInfo(this UITeamItem self, Hero hero)
        {
            self.HeroId = hero.Id;
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }

        public static void UpdateBorder(this UITeamItem self, long heroId)
        {
            self.Image_Border.gameObject.SetActive(self.HeroId == heroId);
        }
    }
}