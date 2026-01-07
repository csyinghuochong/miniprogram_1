using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIArchiveHeroItem))]
    [FriendOf(typeof(UIArchiveHeroItem))]
    public static partial class UIArchiveHeroItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIArchiveHeroItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Image_HeroQuality = rc.Get<GameObject>("Image_HeroQuality").GetComponent<Image>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Transform_HeroStar = rc.Get<GameObject>("Transform_HeroStar").transform;
            self.Button_Click = rc.Get<GameObject>("Button_Click").GetComponent<Button>();
            self.Button_JiFen = rc.Get<GameObject>("Button_JiFen").GetComponent<Button>();
            self.Text_JiFen = rc.Get<GameObject>("Text_JiFen").GetComponent<TMP_Text>();
            self.Text_NotHave = rc.Get<GameObject>("Text_NotHave").GetComponent<TMP_Text>();

            self.Button_Click.AddListener(() => { self.OnButton_Click().Coroutine(); });
            self.Button_JiFen.AddListener(() => { self.OnButton_JiFen().Coroutine(); });
        }

        public static async ETTask UpdateInfo(this UIArchiveHeroItem self, ArchiveHero archiveHero)
        {
            self.HeroConfigId = archiveHero.HeroConfigId;

            self.Transform_HeroStar.gameObject.SetActive(true);
            self.Text_NotHave.gameObject.SetActive(false);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(self.HeroConfigId);
            self.Text_HeroName.text = heroConfig.HeroName;
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            UICommonHelper.HideChild(self.Transform_HeroStar.gameObject);
            for (int i = 0; i < heroConfig.HeroUpStarNeed.Length - 1; i++)
            {
                if (i < self.Transform_HeroStar.childCount)
                {
                    self.Transform_HeroStar.GetChild(i).gameObject.SetActive(true);
                    self.Transform_HeroStar.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    GameObject prefab = self.Transform_HeroStar.GetChild(0).gameObject;
                    GameObject go = UnityEngine.Object.Instantiate(prefab, self.Transform_HeroStar);
                    go.SetActive(true);
                }

                GameObject star = self.Transform_HeroStar.GetChild(i).GetChild(0).gameObject;
                star.SetActive(archiveHero.Star > i);
            }

            self.ShowJiFen();
        }

        public static async ETTask UpdateInfo(this UIArchiveHeroItem self, int heroConfigId)
        {
            self.HeroConfigId = heroConfigId;

            self.Transform_HeroStar.gameObject.SetActive(false);
            self.Text_NotHave.gameObject.SetActive(true);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(heroConfigId);
            self.Text_HeroName.text = heroConfig.HeroName;
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);

            self.ShowJiFen();
        }

        private static void ShowJiFen(this UIArchiveHeroItem self)
        {
            Hero hero = self.Root().GetComponent<HeroComponentC>().GetHeroByConfigId(self.HeroConfigId);
            if (hero == null)
            {
                self.Button_JiFen.gameObject.SetActive(false);
                return;
            }

            ArchiveHero archiveHero = self.Root().GetComponent<ArchiveComponentC>().GetArchiveHero(self.HeroConfigId);
            if (archiveHero == null)
            {
                int score = ConfigData.ArchiveHeroAddScore + hero.Star * ConfigData.ArchiveHeroStarAddScore;
                self.Text_JiFen.SetTextFormat("+{0}积分", score);
                self.Button_JiFen.gameObject.SetActive(true);
                return;
            }

            if (archiveHero.Star < hero.Star)
            {
                int score = ConfigData.ArchiveHeroAddScore + (hero.Star - archiveHero.Star) * ConfigData.ArchiveHeroStarAddScore;
                self.Text_JiFen.SetTextFormat("+{0}积分", score);
                self.Button_JiFen.gameObject.SetActive(true);
            }
        }

        private static async ETTask OnButton_Click(this UIArchiveHeroItem self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroDetails);
            UIHeroDetailsComponent uiHeroDetailsComponent = ui.GetComponent<UIHeroDetailsComponent>();
            uiHeroDetailsComponent.UpdateHeroDetails(self.HeroConfigId).Coroutine();
        }

        private static async ETTask OnButton_JiFen(this UIArchiveHeroItem self)
        {
            int error = await ClientArchiveHelper.ActiveArchiveHero(self.Root(), self.HeroConfigId);
            if (error == ErrorCode.ERR_Success)
            {
                UIArchiveComponent uIArchiveComponent = self.GetParent<UIArchiveComponent>();
                uIArchiveComponent.SetShowType(uIArchiveComponent.CurrentPage);
            }
        }
    }
}