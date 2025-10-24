using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroFormationComponent))]
    [FriendOf(typeof(UIHeroFormationComponent))]
    public static partial class UIHeroFormationComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroFormationComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_TotalCP = rc.Get<GameObject>("Text_TotalCP").GetComponent<TMP_Text>();
            self.UIFormationSlotItem_1 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_1"));
            self.UIFormationSlotItem_2 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_2"));
            self.UIFormationSlotItem_3 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_3"));
            self.UIFormationSlotItem_4 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_4"));
            self.UIFormationSlotItem_5 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_5"));
            self.UIFormationSlotItem_6 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_6"));
            self.UIFormationSlotItem_7 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_7"));
            self.UIFormationSlotItem_8 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_8"));
            self.UIFormationSlotItem_9 = self.AddChild<UIFormationSlotItem, GameObject>(rc.Get<GameObject>("UIFormationSlotItem_9"));
            self.Button_Type_All = rc.Get<GameObject>("Button_Type_All").GetComponent<Button>();
            self.Button_Type_Melee = rc.Get<GameObject>("Button_Type_Melee").GetComponent<Button>();
            self.Button_Type_Ranged = rc.Get<GameObject>("Button_Type_Ranged").GetComponent<Button>();
            self.Content_UIFormationHeroItem = rc.Get<GameObject>("Content_UIFormationHeroItem").transform;
            self.UIFormationHeroItem = rc.Get<GameObject>("UIFormationHeroItem");
            self.UIFormationHeroItem.SetActive(false);
            
            self.Button_Type_All.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Type_Melee.onClick.AddListener(() => { self.SetShowType(2); });
            self.Button_Type_Ranged.onClick.AddListener(() => { self.SetShowType(3); });
        }

        [EntitySystem]
        private static void Destroy(this UIHeroFormationComponent self)
        {
            self.UIFormationHeroItemList.Clear();
            self.UIFormationHeroItemList = null;
        }

        public static void UpdateSlotItemList(this UIHeroFormationComponent self)
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
        
        public static void SetShowType(this UIHeroFormationComponent self, int page)
        {
            self.Button_Type_All.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_All.transform.Find("Image_Off").gameObject.SetActive(page != 1);
            self.Button_Type_Melee.transform.Find("Image_On").gameObject.SetActive(page == 2);
            self.Button_Type_Melee.transform.Find("Image_Off").gameObject.SetActive(page != 2);
            self.Button_Type_Ranged.transform.Find("Image_On").gameObject.SetActive(page == 3);
            self.Button_Type_Ranged.transform.Find("Image_Off").gameObject.SetActive(page != 3);

            self.UpdateHeroList(page);
        }

        public static void UpdateHeroList(this UIHeroFormationComponent self, int page)
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

        public static async ETTask OnSelectHero(this UIHeroFormationComponent self, long heroId)
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