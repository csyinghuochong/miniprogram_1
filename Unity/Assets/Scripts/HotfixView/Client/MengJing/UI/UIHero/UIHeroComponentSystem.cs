using System.Collections.Generic;
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

            self.Content_UITeamItem = rc.Get<GameObject>("Content_UITeamItem").transform;
            self.UITeamItem = rc.Get<GameObject>("UITeamItem");
            self.UITeamItem.SetActive(false);
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Formation = rc.Get<GameObject>("Button_Formation").GetComponent<Button>();

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHero); });
            self.Button_Hero.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroList).Coroutine(); });
            self.Button_Formation.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFormation).Coroutine(); });

            self.UpdateHeroList();
        }

        [EntitySystem]
        private static void Destroy(this UIHeroComponent self)
        {
            self.UITeamItemList.Clear();
            self.UITeamItemList = null;
        }

        public static void UpdateHeroList(this UIHeroComponent self)
        {
            // HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            // List<long> currentFormation = heroComponentC.Formation;
            // List<Hero> heroList = new List<Hero>();
            // foreach (long id in currentFormation)
            // {
            //     Hero hero = heroComponentC.GetHero(id);
            //     if (hero != null)
            //     {
            //         heroList.Add(hero);
            //     }
            // }
            //
            // while (self.UITeamItemList.Count < heroList.Count)
            // {
            //     GameObject go = UnityEngine.Object.Instantiate(self.UITeamItem, self.Content_UITeamItem);
            //     UITeamItem newItem = self.AddChild<UITeamItem, GameObject>(go);
            //     self.UITeamItemList.Add(newItem);
            // }
            //
            // for (int i = 0; i < heroList.Count; i++)
            // {
            //     self.UITeamItemList[i].UpdateInfo(heroList[i]).Coroutine();
            //     self.UITeamItemList[i].GameObject.SetActive(true);
            // }
            //
            // for (int i = heroList.Count; i < self.UITeamItemList.Count; i++)
            // {
            //     self.UITeamItemList[i].GameObject.SetActive(false);
            // }
        }
    }
}