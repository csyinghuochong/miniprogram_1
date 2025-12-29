using System.Collections.Generic;
using Cysharp.Text;
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

            self.Text_FormationCount = rc.Get<GameObject>("Text_FormationCount").GetComponent<TMP_Text>();
            self.Text_TotalCP = rc.Get<GameObject>("Text_TotalCP").GetComponent<TMP_Text>();
            self.Transform_UIFormationSlotItemList = rc.Get<GameObject>("Transform_UIFormationSlotItemList").transform;
            for (int i = 0; i < 9; i++)
            {
                UIFormationSlotItem uiFormationSlotItem = self.AddChild<UIFormationSlotItem, GameObject>(self.Transform_UIFormationSlotItemList
                        .Find(ZString.Format("UIFormationSlotItem_{0}", i + 1)).gameObject);
                self.UIFormationSlotItemList.Add(uiFormationSlotItem);
            }

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

        public static void UpdateOther(this UIHeroFormationComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            List<long> currentFormation = heroComponentC.Formation;

            long totalCP = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<NumericComponentC>().GetAsLong(NumericType.CombatPower);

            //获取当前上阵英雄数量
            int currentHeroCount = 0;
            for (int i = 0; i < currentFormation.Count; i++)
            {
                if (currentFormation[i] == 0)
                {
                    continue;
                }

                currentHeroCount += 1;
            }

            heroComponentC.currentTeamHeroCount = currentHeroCount;

            self.Text_FormationCount.SetTextFormat("上阵数:{0}/{1}", heroComponentC.currentTeamHeroCount, heroComponentC.maxTeamHeroCount);
            self.Text_TotalCP.SetText(totalCP);
        }

        public static void UpdateSlotItemList(this UIHeroFormationComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            for (int i = 0; i < 9; i++)
            {
                self.UIFormationSlotItemList[i].UpdateInfo(heroComponentC.GetHero(heroComponentC.Formation[i]), i + 1).Coroutine();
            }
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

            if (heroComponentC.currentTeamHeroCount >= heroComponentC.maxTeamHeroCount)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("上阵英雄数量已满");
                return;
            }

            for (int i = 0; i < currentFormation.Count; i++)
            {
                if (currentFormation[i] == 0)
                {
                    // 有空位直接上阵
                    int error = await ClientHeroHelper.SetHeroFormation(self.Root(), 0, heroId, i + 1);
                    if (error == ErrorCode.ERR_Success)
                    {
                        heroComponentC.currentTeamHeroCount += 1;
                        self.UpdateOther();
                        self.UpdateSlotItemList();
                        self.UpdateHeroList(self.ShowHeroType);
                    }

                    return;
                }
            }
        }

        public static async ETTask OnSelectHero(this UIHeroFormationComponent self, long heroId, int slotIndex)
        {
            int error = await ClientHeroHelper.SetHeroFormation(self.Root(), 0, heroId, slotIndex);
            if (error == ErrorCode.ERR_Success)
            {
                self.UpdateOther();
                self.UpdateSlotItemList();
                self.UpdateHeroList(self.ShowHeroType);
            }
        }

        public static async ETTask OnUnloadHero(this UIHeroFormationComponent self, long heroId, int slotIndex)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
            if (heroComponentC.currentTeamHeroCount <= 1)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("最少上阵一个英雄");
                return;
            }

            int error = await ClientHeroHelper.SetHeroFormation(self.Root(), 1, heroId, slotIndex);
            if (error == ErrorCode.ERR_Success)
            {
                heroComponentC.currentTeamHeroCount -= 1;
                self.UpdateOther();
                self.UpdateSlotItemList();
                self.UpdateHeroList(self.ShowHeroType);
            }
        }
    }
}