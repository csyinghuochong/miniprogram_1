using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroListComponent))]
    [FriendOf(typeof(UIHeroListComponent))]
    public static partial class UIHeroListComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroListComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_HaveHeroCount = rc.Get<GameObject>("Text_HaveHeroValue").GetComponent<TMP_Text>();
            self.Button_Type_All = rc.Get<GameObject>("Button_Type_All").GetComponent<Button>();
            self.Button_Type_Melee = rc.Get<GameObject>("Button_Type_Melee").GetComponent<Button>();
            self.Button_Type_Ranged = rc.Get<GameObject>("Button_Type_Ranged").GetComponent<Button>();
            self.Content_UIHeroItem = rc.Get<GameObject>("Content_UIHeroItem").GetComponent<Transform>();
            self.UIHeroItem = rc.Get<GameObject>("UIHeroItem");
            self.UIHeroItem.gameObject.SetActive(false);

            self.Button_Type_All.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_Melee.onClick.AddListener(() => { self.SetShowType(2); });
            self.Button_Type_Ranged.onClick.AddListener(() => { self.SetShowType(3); });
        }

        [EntitySystem]
        private static void Destroy(this UIHeroListComponent self)
        {
            self.UIHeroItemList.Clear();
            self.UIHeroItem = null;
        }

        public static void SetShowType(this UIHeroListComponent self, int page)
        {
            self.Button_Type_All.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_All.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_Melee.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_Melee.transform.Find("Image_Off").gameObject.SetActive(page != 2);
            self.Button_Type_Ranged.transform.Find("Image_On").gameObject.SetActive(page == 3);
            self.Button_Type_Ranged.transform.Find("Image_Off").gameObject.SetActive(page != 3);

            self.UpdateHeroList(page);
        }

        public static void UpdateHaveHeroCount(this UIHeroListComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            int heroCount = heroComponentC.GetAllHeroCount();
            int allHeroCount = HeroConfigCategory.Instance.DataMap.Count;

            self.Text_HaveHeroCount.SetTextFormat("{0}/{1}", heroCount, allHeroCount);
        }

        private static void UpdateHeroList(this UIHeroListComponent self, int page)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();

            List<Hero> heroList = null;
            if (page == 1)
            {
                heroList = heroComponentC.GetAllHero();
            }
            else if (page == 2)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Melee);
            }
            else if (page == 3)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Ranged);
            }
            else
            {
                return;
            }

            while (self.UIHeroItemList.Count < heroList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIHeroItem, self.Content_UIHeroItem);
                UIHeroItem newItem = self.AddChild<UIHeroItem, GameObject>(go);
                self.UIHeroItemList.Add(newItem);
            }

            for (int i = 0; i < heroList.Count; i++)
            {
                self.UIHeroItemList[i].UpdateInfo(heroList[i]).Coroutine();
                self.UIHeroItemList[i].GameObject.SetActive(true);
            }

            for (int i = heroList.Count; i < self.UIHeroItemList.Count; i++)
            {
                self.UIHeroItemList[i].GameObject.SetActive(false);
            }
        }
    }
}