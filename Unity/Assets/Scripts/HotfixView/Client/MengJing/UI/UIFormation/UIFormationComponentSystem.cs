using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFormationComponent))]
    [FriendOf(typeof(UIFormationComponent))]
    public static partial class UIFormationComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIFormationComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.UIFormationSlotItem_1 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_1"));
            self.UIFormationSlotItem_2 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_2"));
            self.UIFormationSlotItem_3 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_3"));
            self.UIFormationSlotItem_4 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_4"));
            self.UIFormationSlotItem_5 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_5"));
            self.UIFormationSlotItem_6 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_6"));
            self.UIFormationSlotItem_7 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_7"));
            self.UIFormationSlotItem_8 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_8"));
            self.UIFormationSlotItem_9 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_9"));
            self.Content_UIFormationHeroItem = rc.Get<GameObject>("Content_UIFormationHeroItem").transform;
            self.UIFormationHeroItem = rc.Get<GameObject>("UIFormationHeroItem");
            self.UIFormationHeroItem.SetActive(false);

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIFormation); });

            self.UpdateSlotItemList();
            self.UpdateHeroList(1);
        }

        [EntitySystem]
        private static void Destroy(this UIFormationComponent self)
        {
            self.UIFormationHeroItemList.Clear();
            self.UIFormationHeroItemList = null;
        }

        private static void UpdateSlotItemList(this UIFormationComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            self.UIFormationSlotItem_1.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[0])).Coroutine();
            self.UIFormationSlotItem_2.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[1])).Coroutine();
            self.UIFormationSlotItem_3.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[2])).Coroutine();
            self.UIFormationSlotItem_4.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[3])).Coroutine();
            self.UIFormationSlotItem_5.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[4])).Coroutine();
            self.UIFormationSlotItem_6.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[5])).Coroutine();
            self.UIFormationSlotItem_7.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[6])).Coroutine();
            self.UIFormationSlotItem_8.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[7])).Coroutine();
            self.UIFormationSlotItem_9.UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[8])).Coroutine();
        }

        private static void UpdateHeroList(this UIFormationComponent self, int page)
        {
            self.ShowHeroType = page;
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();

            List<Hero> heroList = null;
            if (page == 1)
            {
                heroList = heroComponentC.GetAllHero();
            }
            else if (page == 2)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Warrior);
            }
            else if (page == 3)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Mage);
            }
            else if (page == 4)
            {
                heroList = heroComponentC.GetHerosByType(HeroType.Archer);
            }
            else
            {
                return;
            }

            while (self.UIFormationHeroItemList.Count < heroList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIFormationHeroItem, self.Content_UIFormationHeroItem);
                UIFormationHeroItem newItem = self.AddChild<UIFormationHeroItem, GameObject>(go);
                self.UIFormationHeroItemList.Add(newItem);
            }

            List<long> currentFormation = heroComponentC.Formation;
            for (int i = 0; i < heroList.Count; i++)
            {
                self.UIFormationHeroItemList[i].UpdateInfo(heroList[i], currentFormation.Contains(heroList[i].Id)).Coroutine();
                self.UIFormationHeroItemList[i].GameObject.SetActive(true);
            }

            for (int i = heroList.Count; i < self.UIFormationHeroItemList.Count; i++)
            {
                self.UIFormationHeroItemList[i].GameObject.SetActive(false);
            }
        }

        public static async ETTask OnSelectHero(this UIFormationComponent self, long heroId)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            List<long> currentFormation = heroComponentC.Formation;
            for (int i = 0; i < currentFormation.Count; i++)
            {
                if (currentFormation[i] == 0)
                {
                    // 有空位直接上阵
                    int error = await ClientHeroHelper.SetHeroFormation(self.Root(), 0, heroId, i + 1);
                    if (error == ErrorCode.ERR_Success)
                    {
                        self.UpdateSlotItemList();
                        self.UpdateHeroList(self.ShowHeroType);
                    }

                    return;
                }
            }
        }
    }
}