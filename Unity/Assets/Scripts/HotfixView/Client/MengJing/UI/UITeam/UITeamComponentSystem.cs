using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class HeroFormationUpdate_UpdateUITeam : AEvent<Scene, HeroFormationUpdate>
    {
        protected override async ETTask Run(Scene root, HeroFormationUpdate args)
        {
            UI ui = root.GetComponent<UIComponent>().Get(UIType.UITeam);
            if (ui == null)
            {
                return;
            }

            UITeamComponent uiTeamComponent = ui.GetComponent<UITeamComponent>();
            uiTeamComponent.UpdateHeroList();
        }
    }

    [EntitySystemOf(typeof(UITeamComponent))]
    [FriendOf(typeof(UITeamComponent))]
    public static partial class UITeamComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UITeamComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Content_UITeamItem = rc.Get<GameObject>("Content_UITeamItem").transform;
            self.UITeamItem = rc.Get<GameObject>("UITeamItem");
            self.UITeamItem.SetActive(false);
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Formation = rc.Get<GameObject>("Button_Formation").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UITeam); });
            self.Button_Hero.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHero).Coroutine(); });
            self.Button_Formation.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFormation).Coroutine(); });

            self.UpdateHeroList();
        }

        [EntitySystem]
        private static void Destroy(this UITeamComponent self)
        {
            self.UITeamItemList.Clear();
            self.UITeamItemList = null;
        }

        public static void UpdateHeroList(this UITeamComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            List<long> currentFormation = heroComponentC.GetFormation(heroComponentC.CurrentFormationIndex);
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
    }
}