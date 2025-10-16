using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class HeroFormationUpdate_UpdateUIHero : AEvent<Scene, HeroFormationUpdate>
    {
        protected override async ETTask Run(Scene root, HeroFormationUpdate args)
        {
            UI ui = root.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();
            uiHeroComponent.UpdateHeroList();
        }
    }

    [EntitySystemOf(typeof(UIHeroComponent))]
    [FriendOf(typeof(UIHeroComponent))]
    public static partial class UITeamComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.UIHeroInfo = rc.Get<GameObject>("Text_HeroExp");
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Text_HeroCP = rc.Get<GameObject>("Text_HeroCP").GetComponent<TMP_Text>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Text_HeroLv = rc.Get<GameObject>("Text_HeroLv").GetComponent<TMP_Text>();
            self.Slider_HeroExp = rc.Get<GameObject>("Slider_HeroExp").GetComponent<Slider>();
            self.Text_HeroExp = rc.Get<GameObject>("Text_HeroExp").GetComponent<TMP_Text>();
            self.Content_UITeamItem = rc.Get<GameObject>("Content_UITeamItem").transform;
            self.UITeamItem = rc.Get<GameObject>("UITeamItem");
            self.UITeamItem.SetActive(false);
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Formation = rc.Get<GameObject>("Button_Formation").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHero); });
            self.Button_Hero.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroList).Coroutine(); });
            self.Button_Formation.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFormation).Coroutine(); });

            self.UpdateHeroList();
            self.SelectFirstHero();
        }

        [EntitySystem]
        private static void Destroy(this UIHeroComponent self)
        {
            self.UITeamItemList.Clear();
            self.UITeamItemList = null;
        }

        public static void UpdateHeroList(this UIHeroComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            List<long> currentFormation = heroComponentC.Formation;
            List<Hero> heroList = new List<Hero>();
            foreach (long id in currentFormation)
            {
                Hero hero = heroComponentC.GetHero(id);
                if (hero != null)
                {
                    heroList.Add(hero);
                }
            }

            while (self.UITeamItemList.Count < heroList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UITeamItem, self.Content_UITeamItem);
                UITeamItem newItem = self.AddChild<UITeamItem, GameObject>(go);
                self.UITeamItemList.Add(newItem);
            }

            for (int i = 0; i < heroList.Count; i++)
            {
                self.UITeamItemList[i].UpdateInfo(heroList[i]).Coroutine();
                self.UITeamItemList[i].GameObject.SetActive(true);
            }

            for (int i = heroList.Count; i < self.UITeamItemList.Count; i++)
            {
                self.UITeamItemList[i].GameObject.SetActive(false);
            }
        }

        private static void SelectFirstHero(this UIHeroComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            self.SelectHero(heroComponentC.Formation[0]);
        }

        public static void SelectHero(this UIHeroComponent self, long heroId)
        {
            self.CurrentHeroId = heroId;

            foreach (UITeamItem item in self.UITeamItemList)
            {
                item.UpdateBorder(heroId);
            }

            self.UpdateHeroInfo().Coroutine();
        }

        public static async ETTask UpdateHeroInfo(this UIHeroComponent self)
        {
            if (self.CurrentHeroId == 0)
            {
                self.UIHeroInfo.SetActive(false);
                return;
            }

            HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
            Hero hero = heroComponent.GetHero(self.CurrentHeroId);
            if (hero == null)
            {
                self.UIHeroInfo.SetActive(false);
                return;
            }

            self.UIHeroInfo.SetActive(true);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            self.Text_HeroLv.SetTextFormat("等级：{0}", hero.Lv);
            int maxExp = 100; // 暂时
            self.Slider_HeroExp.value = hero.Exp * 1f / maxExp;
            self.Text_HeroExp.SetTextFormat("{0}/{1}", hero.Exp, maxExp);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
        }
    }
}