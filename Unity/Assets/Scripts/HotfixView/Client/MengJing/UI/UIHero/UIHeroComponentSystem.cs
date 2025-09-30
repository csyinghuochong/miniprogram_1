using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroComponent))]
    [FriendOf(typeof(UIHeroComponent))]
    public static partial class UIHeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Type_All = rc.Get<GameObject>("Button_Type_All").GetComponent<Button>();
            self.Button_Type_Warrior = rc.Get<GameObject>("Button_Type_Warrior").GetComponent<Button>();
            self.Button_Type_Mage = rc.Get<GameObject>("Button_Type_Mage").GetComponent<Button>();
            self.Button_Type_Archer = rc.Get<GameObject>("Button_Type_Archer").GetComponent<Button>();
            self.Content_UIHeroItem = rc.Get<GameObject>("Content_UIHeroItem").GetComponent<Transform>();
            self.UIHeroItem = rc.Get<GameObject>("UIHeroItem");
            self.UIHeroItem.gameObject.SetActive(false);

            self.Button_Type_All.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_Warrior.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_Mage.onClick.AddListener(() => { self.SetShowType(2); });
            self.Button_Type_Archer.onClick.AddListener(() => { self.SetShowType(3); });
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHero); });

            self.SetShowType(0);
        }

        [EntitySystem]
        private static void Destroy(this UIHeroComponent self)
        {
            self.UIHeroItemList.Clear();
            self.UIHeroItem = null;
        }

        private static void SetShowType(this UIHeroComponent self, int page)
        {
            self.Button_Type_All.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_All.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_Warrior.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_Warrior.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_Mage.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_Mage.transform.Find("Image_Off").gameObject.SetActive(page != 2);
            self.Button_Type_Archer.transform.Find("Image_On").gameObject.SetActive(page == 3);
            self.Button_Type_Archer.transform.Find("Image_Off").gameObject.SetActive(page != 3);

            self.UpdateHeroList(page);
        }

        private static void UpdateHeroList(this UIHeroComponent self, int page)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();

            List<Hero> heroList = null;
            if (page == 0)
            {
                heroList = heroComponentC.GetAllHero();
            }
            else if (page == 1)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Warrior);
            }
            else if (page == 2)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Mage);
            }
            else if (page == 3)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Archer);
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