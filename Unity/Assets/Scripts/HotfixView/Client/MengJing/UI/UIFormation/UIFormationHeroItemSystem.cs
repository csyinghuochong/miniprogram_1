using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Text;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFormationHeroItem))]
    [FriendOf(typeof(UIFormationHeroItem))]
    public static partial class UIFormationHeroItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIFormationHeroItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Image_Selected = rc.Get<GameObject>("Image_Selected").GetComponent<Image>();
            self.Text_Lv = rc.Get<GameObject>("Text_Lv").GetComponent<TMP_Text>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();

            self.Button_Click.onClick.AddListener(() => { self.OnClick(); });
        }

        public static async ETTask UpdateInfo(this UIFormationHeroItem self, Hero hero, bool selected)
        {
            if (hero == null)
            {
                self.HeroId = 0;
                return;
            }

            self.HeroId = hero.Id;

            self.Text_Lv.SetText(hero.Lv);
            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            self.Image_Selected.gameObject.SetActive(selected);
            self.Button_Click.gameObject.SetActive(!selected);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }

        private static void OnClick(this UIFormationHeroItem self)
        {
            self.GetParent<UIFormationComponent>().OnSelectHero(self.HeroId).Coroutine();
        }
    }
}