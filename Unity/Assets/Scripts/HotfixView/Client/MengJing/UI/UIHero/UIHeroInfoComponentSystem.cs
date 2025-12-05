using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroInfoComponent))]
    [FriendOf(typeof(UIHeroInfoComponent))]
    public static partial class UIHeroInfoComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroInfoComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            
            self.UIHeroInfo_1 = rc.Get<GameObject>("UIHeroInfo_1");
            self.Spine_HeroModel = rc.Get<GameObject>("Spine_HeroModel").transform;
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Text_HeroCP = rc.Get<GameObject>("Text_HeroCP").GetComponent<TMP_Text>();
            self.Text_HeroLv = rc.Get<GameObject>("Text_HeroLv").GetComponent<TMP_Text>();
            self.Slider_HeroExp = rc.Get<GameObject>("Slider_HeroExp").GetComponent<Slider>();
            self.Text_HeroExp = rc.Get<GameObject>("Text_HeroExp").GetComponent<TMP_Text>();
            self.UIEquipmentItem_1 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_1"));
            self.UIEquipmentItem_2 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_2"));
            self.UIEquipmentItem_3 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_3"));
            self.UIEquipmentItem_4 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_4"));
            self.UIEquipmentItem_5 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_5"));
            self.UIEquipmentItem_6 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_6"));
            self.Transform_HeroStar = rc.Get<GameObject>("Transform_HeroStar").transform;
            self.Transform_HeroStar.GetChild(0).gameObject.SetActive(false);
            self.UIHeroInfo_2 = rc.Get<GameObject>("UIHeroInfo_2");
            self.Content_UIBaseAttributeItem = rc.Get<GameObject>("Content_UIBaseAttributeItem").transform;
            self.UIBaseAttributeItem = rc.Get<GameObject>("UIBaseAttributeItem");
            self.UIBaseAttributeItem.SetActive(false);
            self.Content_UIOtherAttributeItem = rc.Get<GameObject>("Content_UIOtherAttributeItem").transform;
            self.UIOtherAttributeItem = rc.Get<GameObject>("UIOtherAttributeItem");
            self.UIOtherAttributeItem.SetActive(false);
            self.Content_UISkillItem = rc.Get<GameObject>("Content_UISkillItem").transform;
            self.UISkillItem = rc.Get<GameObject>("UISkillItem");
            self.UISkillItem.SetActive(false);
            self.Button_XiangXi = rc.Get<GameObject>("Button_XiangXi").GetComponent<Button>();
            self.Button_ShengXing = rc.Get<GameObject>("Button_ShengXing").GetComponent<Button>();
            self.Button_ShengJi = rc.Get<GameObject>("Button_ShengJi").GetComponent<Button>();
            self.ScrollView_ItemList = rc.Get<GameObject>("ScrollView_ItemList");
            self.ScrollView_ItemList.SetActive(false);
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").GetComponent<Transform>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Content_UITeamItem = rc.Get<GameObject>("Content_UITeamItem").transform;
            self.UITeamItem = rc.Get<GameObject>("UITeamItem");
            self.UITeamItem.SetActive(false);

            self.UIEquipmentItem_1.EquipSlotType = EquipSlotType.Toukui;
            self.UIEquipmentItem_2.EquipSlotType = EquipSlotType.Yifu;
            self.UIEquipmentItem_3.EquipSlotType = EquipSlotType.Kuzi;
            self.UIEquipmentItem_4.EquipSlotType = EquipSlotType.Xiezi;
            self.UIEquipmentItem_5.EquipSlotType = EquipSlotType.Xianglian;
            self.UIEquipmentItem_6.EquipSlotType = EquipSlotType.Wuqi;
            self.Button_XiangXi.AddListener(() => { self.OnButton_XiangXi().Coroutine(); });
            self.Button_ShengXing.AddListener(() => { self.OnButton_ShengXing().Coroutine(); });
            self.Button_ShengJi.AddListener(() => { self.OnButton_ShengJi().Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIHeroInfoComponent self)
        {
            self.UISkillItemList.Clear();
            self.UISkillItemList = null;
            self.UICommonItemList.Clear();
            self.UICommonItemList = null;
            self.UITeamItemList.Clear();
            self.UITeamItemList = null;
        }
        
        public static void UpdateHeroList(this UIHeroInfoComponent self)
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

        public static void SelectFirstHero(this UIHeroInfoComponent self)
        {
            HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();

            for (int i = 0; i < heroComponentC.Formation.Count; i++)
            {
                if (heroComponentC.Formation[i] != 0)
                {
                    self.SelectHero(heroComponentC.Formation[i]);
                    break;
                }
            }
        }

        public static void SelectHero(this UIHeroInfoComponent self, long heroId)
        {
            self.CurrentHeroId = heroId;

            self.UIHeroInfo_1.SetActive(true);
            self.UIHeroInfo_2.SetActive(true);
            self.ScrollView_ItemList.SetActive(false);

            foreach (UITeamItem item in self.UITeamItemList)
            {
                item.UpdateBorder(heroId);
            }

            self.UpdateHeroInfo().Coroutine();
        }

        private static async ETTask OnEquipmentClick(this UIHeroInfoComponent self, EquipSlotType equipSlotType)
        {
            self.ShowItemList();

            HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
            Hero hero = heroComponent.GetHero(self.CurrentHeroId);
            if (hero.Equipments[(int)equipSlotType] == 0)
            {
                return;
            }

            UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
            if (uI != null)
            {
                InventoryComponentC inventoryComponent = self.Root().GetComponent<InventoryComponentC>();
                uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                {
                    Item = inventoryComponent.GetItem(hero.Equipments[(int)equipSlotType]),
                    UIItemTipOpType = UIItemTipOpType.UIHero_TakeOff,
                    HeroId = self.CurrentHeroId
                });
            }
        }

        public static async ETTask UpdateHeroInfo(this UIHeroInfoComponent self)
        {
            if (self.CurrentHeroId == 0)
            {
                self.UIHeroInfo_1.SetActive(false);
                self.UIHeroInfo_2.SetActive(false);
                return;
            }

            HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
            Hero hero = heroComponent.GetHero(self.CurrentHeroId);
            if (hero == null)
            {
                self.UIHeroInfo_1.SetActive(false);
                self.UIHeroInfo_2.SetActive(false);
                return;
            }

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            self.Text_HeroCP.SetText(hero.NumericDic[NumericType.CombatPower]);

            string path = ABPathHelper.GetUIUnitPath(ABUnitType.Hero, heroConfig.HeroModelID);
            GameObject model = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            model.transform.localScale = new Vector3(1f, 1f, 1f);
            UICommonHelper.DestoryChild(self.Spine_HeroModel.gameObject);
            UnityEngine.Object.Instantiate(model, self.Spine_HeroModel);

            self.Text_HeroLv.SetTextFormat("LV.{0}", hero.Lv);
            int maxExp = ExpConfigCategory.Instance.Get(hero.Lv).HeroUpExp;
            self.Slider_HeroExp.value = hero.Exp * 1f / maxExp;
            self.Text_HeroExp.SetTextFormat("{0}/{1}", hero.Exp, maxExp);

            // 星级
            UICommonHelper.HideChild(self.Transform_HeroStar.gameObject);
            for (int i = 0; i < hero.Star; i++)
            {
                if (i < self.Transform_HeroStar.childCount)
                {
                    self.Transform_HeroStar.GetChild(i).gameObject.SetActive(true);
                }
                else
                {
                    GameObject prefab = self.Transform_HeroStar.GetChild(0).gameObject;
                    GameObject go = UnityEngine.Object.Instantiate(prefab, self.Transform_HeroStar);
                    go.SetActive(true);
                }
            }

            // 装备
            self.UIEquipmentItem_1.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_2.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_3.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_4.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_5.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_6.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();

            // 基础属性
            self.ShowBaseStatItem(1, 1, "生命", hero.NumericDic[NumericType.Base_MaxHp_Base].ToString());
            self.ShowBaseStatItem(2, 2, "攻击", ZString.Format("{0}-{1}", hero.NumericDic[NumericType.Base_MinAct_Base], hero.NumericDic[NumericType.Base_MaxAct_Base]));
            self.ShowBaseStatItem(3, 1, "物防", ZString.Format("{0}-{1}", hero.NumericDic[NumericType.Base_MinDef_Base], hero.NumericDic[NumericType.Base_MaxDef_Base]));
            self.ShowBaseStatItem(4, 1, "魔防", ZString.Format("{0}-{1}", hero.NumericDic[NumericType.Base_MinAdf_Base], hero.NumericDic[NumericType.Base_MaxAdf_Base]));

            // 特殊属性
            self.ShowOtherStatItem(1, 2, "暴击", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_Cri_Base] / 10000f * 100f));
            self.ShowOtherStatItem(2, 1, "抗暴", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_ReCri_Base] / 10000f * 100f));

            // 技能
            while (self.UISkillItemList.Count < heroConfig.UnlockSkillInfos.Length)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UISkillItem, self.Content_UISkillItem);
                UISkillItem newItem = self.AddChild<UISkillItem, GameObject>(go);
                self.UISkillItemList.Add(newItem);
            }

            for (int i = 0; i < heroConfig.UnlockSkillInfos.Length; i++)
            {
                self.UISkillItemList[i].UpdateInfo(heroConfig.UnlockSkillInfos[i], hero.Star).Coroutine();
                self.UISkillItemList[i].GameObject.SetActive(true);
            }

            for (int i = heroConfig.UnlockSkillInfos.Length; i < self.UISkillItemList.Count; i++)
            {
                self.UISkillItemList[i].GameObject.SetActive(false);
            }
        }
        
        private static void ShowBaseStatItem(this UIHeroInfoComponent self, int index, int icon, string name, string value)
        {
            Transform item = null;
            if (self.Content_UIBaseAttributeItem.childCount <= index)
            {
                item = UnityEngine.Object.Instantiate(self.UIBaseAttributeItem, self.Content_UIBaseAttributeItem).transform;
            }
            else
            {
                item = self.Content_UIBaseAttributeItem.GetChild(index);
            }

            if (item == null)
            {
            }

            item.gameObject.SetActive(true);

            ReferenceCollector rc = item.GetComponent<ReferenceCollector>();
            //显示抗性图标
            if (icon == 1)
            {
                rc.Get<GameObject>("Image_IconAtk").GetComponent<Image>().gameObject.SetActive(false);
            }
            //显示攻击图标
            else if (icon == 2)
            {
                rc.Get<GameObject>("Image_IconDef").GetComponent<Image>().gameObject.SetActive(false);
            }
            rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>().SetText(name);
            rc.Get<GameObject>("Text_Value").GetComponent<TMP_Text>().SetText(value);
        }

        private static void ShowOtherStatItem(this UIHeroInfoComponent self, int index, int icon, string name, string value)
        {
            Transform item = null;
            if (self.Content_UIOtherAttributeItem.childCount <= index)
            {
                item = UnityEngine.Object.Instantiate(self.UIOtherAttributeItem, self.Content_UIOtherAttributeItem).transform;
            }
            else
            {
                item = self.Content_UIOtherAttributeItem.GetChild(index);
            }

            item.gameObject.SetActive(true);

            ReferenceCollector rc = item.GetComponent<ReferenceCollector>();
            if (icon == 1)
            {
                rc.Get<GameObject>("Image_IconAtk").GetComponent<Image>().gameObject.SetActive(false);
            }
            else if (icon == 2)
            {
                rc.Get<GameObject>("Image_IconDef").GetComponent<Image>().gameObject.SetActive(false);
            }
            rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>().SetText(name);
            rc.Get<GameObject>("Text_Value").GetComponent<TMP_Text>().SetText(value);
        }

        public static void UpdateItemList(this UIHeroInfoComponent self)
        {
            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = inventoryComponentC.GetAllItems();
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                Item item = itemList[i];
                if (item.ContainerType != (int)InventoryContainerType.Bag && item.ContainerType != (int)InventoryContainerType.HeroEquipment)
                {
                    itemList.RemoveAt(i);
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                if (itemConfig.ItemType != ItemType.Equipment)
                {
                    itemList.RemoveAt(i);
                }
            }

            while (self.UICommonItemList.Count < itemList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
            for (int i = 0; i < itemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(itemList[i], (item) => { self.OnItemClick(item).Coroutine(); }).Coroutine();
                self.UICommonItemList[i].Image_Equipped.gameObject.SetActive(heroComponent.GetHeroIdByEquipmentId(itemList[i].Id) != 0);
                self.UICommonItemList[i].GameObject.SetActive(true);
            }

            for (int i = itemList.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].GameObject.SetActive(false);
            }
        }

        private static async ETTask OnItemClick(this UIHeroInfoComponent self, Item item)
        {
            UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
            if (uI != null)
            {
                HeroComponentC heroComponent = self.Root().GetComponent<HeroComponentC>();
                if (heroComponent.GetHeroIdByEquipmentId(item.Id) != 0)
                {
                    uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                    {
                        Item = item,
                        UIItemTipOpType = UIItemTipOpType.UIHero_TakeOff,
                        HeroId = heroComponent.GetHeroIdByEquipmentId(item.Id)
                    });
                }
                else
                {
                    uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                    {
                        Item = item,
                        UIItemTipOpType = UIItemTipOpType.UIHero_Wear,
                        HeroId = self.CurrentHeroId
                    });
                }
            }
        }

        public static void ShowItemList(this UIHeroInfoComponent self)
        {
            self.UIHeroInfo_2.SetActive(false);
            self.ScrollView_ItemList.SetActive(true);
            self.UpdateItemList();
        }

        public static async ETTask OnButton_XiangXi(this UIHeroInfoComponent self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroAttributes);
            UIHeroAttributesComponent uiHeroAttributesComponent = ui.GetComponent<UIHeroAttributesComponent>();
            uiHeroAttributesComponent.UpdateAttributes(self.CurrentHeroId);
        }

        private static async ETTask OnButton_ShengXing(this UIHeroInfoComponent self)
        {
            if (self.CurrentHeroId == 0)
            {
                return;
            }

            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroStarUp);
            UIHeroStarUpComponent uiHeroStarUpComponent = ui.GetComponent<UIHeroStarUpComponent>();
            uiHeroStarUpComponent.UpdateInfo(self.CurrentHeroId).Coroutine();
        }

        private static async ETTask OnButton_ShengJi(this UIHeroInfoComponent self)
        {
            if (self.CurrentHeroId == 0)
            {
                return;
            }

            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroLvUp);
            UIHeroLvUpComponent uiHeroLvUpComponent = ui.GetComponent<UIHeroLvUpComponent>();
            uiHeroLvUpComponent.UpdateInfo(self.CurrentHeroId).Coroutine();
        }
    }
}