using System.Collections.Generic;
using Cysharp.Text;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class DataUpdate_UpdateUserData_UpdateUIHero : AEvent<Scene, UpdateUserData>
    {
        protected override async ETTask Run(Scene scene, UpdateUserData args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();

            if (args.UserDataType == UserDataType.Gold)
            {
                uiHeroComponent.UpdateGold();
            }

            if (args.UserDataType == UserDataType.Diamond)
            {
                uiHeroComponent.UpdateDiamond();
            }

            await ETTask.CompletedTask;
        }
    }

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

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class HeroUpdate_UpdateUIHero : AEvent<Scene, HeroUpdate>
    {
        protected override async ETTask Run(Scene root, HeroUpdate args)
        {
            UI ui = root.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();
            uiHeroComponent.UpdateHeroInfo().Coroutine();

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class InventoryUpdate_UIHeroRefresh : AEvent<Scene, InventoryUpdate>
    {
        protected override async ETTask Run(Scene scene, InventoryUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIHero);
            if (ui == null)
            {
                return;
            }

            UIHeroComponent uiHeroComponent = ui.GetComponent<UIHeroComponent>();
            uiHeroComponent.UpdateItemList();

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIHeroComponent))]
    [FriendOf(typeof(UIHeroComponent))]
    public static partial class UIHeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.UIHeroInfo_1 = rc.Get<GameObject>("UIHeroInfo_1");
            self.Spine_HeroModel = rc.Get<GameObject>("Spine_HeroModel").transform;
            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
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
            self.Button_Hero = rc.Get<GameObject>("Button_Hero").GetComponent<Button>();
            self.Button_Formation = rc.Get<GameObject>("Button_Formation").GetComponent<Button>();

            self.UIEquipmentItem_1.EquipSlotType = EquipSlotType.Toukui;
            self.UIEquipmentItem_2.EquipSlotType = EquipSlotType.Yifu;
            self.UIEquipmentItem_3.EquipSlotType = EquipSlotType.Kuzi;
            self.UIEquipmentItem_4.EquipSlotType = EquipSlotType.Xiezi;
            self.UIEquipmentItem_5.EquipSlotType = EquipSlotType.Xianglian;
            self.UIEquipmentItem_6.EquipSlotType = EquipSlotType.Wuqi;
            self.Button_ShengJi.AddListener(() => { self.OnButton_ShengJi().Coroutine(); });
            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHero); });
            self.Button_Hero.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroList).Coroutine(); });
            self.Button_Formation.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFormation).Coroutine(); });

            self.UpdateGold();
            self.UpdateDiamond();
            self.UpdateHeroList();
            self.SelectFirstHero();
        }

        [EntitySystem]
        private static void Destroy(this UIHeroComponent self)
        {
            self.UISkillItemList.Clear();
            self.UISkillItemList = null;
            self.UICommonItemList.Clear();
            self.UICommonItemList = null;
            self.UITeamItemList.Clear();
            self.UITeamItemList = null;
        }
        
        public static void UpdateGold(this UIHeroComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            self.Text_Type_Gold.SetText(userInfoComponent.Gold);
        }

        public static void UpdateDiamond(this UIHeroComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            self.Text_Type_Diamond.SetText(userInfoComponent.Diamond);
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

            self.UIHeroInfo_1.SetActive(true);
            self.UIHeroInfo_2.SetActive(true);
            self.ScrollView_ItemList.SetActive(false);

            foreach (UITeamItem item in self.UITeamItemList)
            {
                item.UpdateBorder(heroId);
            }

            self.UpdateHeroInfo().Coroutine();
        }

        private static async ETTask OnEquipmentClick(this UIHeroComponent self, EquipSlotType equipSlotType)
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
                uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                {
                    ItemId = hero.Equipments[(int)equipSlotType],
                    UIItemTipOpType = UIItemTipOpType.UIHero_TakeOff,
                    HeroId = self.CurrentHeroId
                });
            }
        }

        public static async ETTask UpdateHeroInfo(this UIHeroComponent self)
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
            self.Text_HeroCP.SetTextFormat("战力：{0}", hero.NumericDic[NumericType.CombatPower]);

            string path = ABPathHelper.GetUIUnitPath(ABUnitType.Hero, heroConfig.HeroModelID);
            GameObject model = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<GameObject>(path);
            UICommonHelper.DestoryChild(self.Spine_HeroModel.gameObject);
            UnityEngine.Object.Instantiate(model, self.Spine_HeroModel);

            self.Text_HeroLv.SetTextFormat("等级：{0}", hero.Lv);
            int maxExp = ExpConfigCategory.Instance.Get(hero.Lv).HeroUpExp;
            self.Slider_HeroExp.value = hero.Exp * 1f / maxExp;
            self.Text_HeroExp.SetTextFormat("{0}/{1}", hero.Exp, maxExp);

            // 装备
            self.UIEquipmentItem_1.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_2.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_3.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_4.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_5.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();
            self.UIEquipmentItem_6.UpdateInfo(hero, (type) => { self.OnEquipmentClick(type).Coroutine(); }).Coroutine();

            // 基础属性
            self.ShowBaseStatItem(1, "生命", hero.NumericDic[NumericType.Base_MaxHp_Base].ToString());
            self.ShowBaseStatItem(2, "攻击", hero.NumericDic[NumericType.Base_MaxAct_Base].ToString());
            self.ShowBaseStatItem(3, "物防", hero.NumericDic[NumericType.Base_MaxDef_Base].ToString());
            self.ShowBaseStatItem(4, "魔防", hero.NumericDic[NumericType.Base_MaxAdf_Base].ToString());

            // 特殊属性
            self.ShowOtherStatItem(1, "暴击", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_Cri_Base] / 10000f * 100f));
            self.ShowOtherStatItem(2, "抗暴", ZString.Format("{0:0.#}%", hero.NumericDic[NumericType.Base_ReCri_Base] / 10000f * 100f));

            // 技能
            while (self.UISkillItemList.Count < heroConfig.SkillID.Length)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UISkillItem, self.Content_UISkillItem);
                UISkillItem newItem = self.AddChild<UISkillItem, GameObject>(go);
                self.UISkillItemList.Add(newItem);
            }

            for (int i = 0; i < heroConfig.SkillID.Length; i++)
            {
                self.UISkillItemList[i].UpdateInfo(heroConfig.SkillID[i]).Coroutine();
                self.UISkillItemList[i].GameObject.SetActive(true);
            }

            for (int i = heroConfig.SkillID.Length; i < self.UISkillItemList.Count; i++)
            {
                self.UISkillItemList[i].GameObject.SetActive(false);
            }
        }

        private static void ShowBaseStatItem(this UIHeroComponent self, int index, string name, string value)
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
            rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>().SetText(name);
            rc.Get<GameObject>("Text_Value").GetComponent<TMP_Text>().SetText(value);
        }

        private static void ShowOtherStatItem(this UIHeroComponent self, int index, string name, string value)
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
            rc.Get<GameObject>("Text_Name").GetComponent<TMP_Text>().SetText(name);
            rc.Get<GameObject>("Text_Value").GetComponent<TMP_Text>().SetText(value);
        }

        public static void UpdateItemList(this UIHeroComponent self)
        {
            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = inventoryComponentC.GetItemsByType(ItemType.Equipment, InventoryContainerType.Bag);

            while (self.UICommonItemList.Count < itemList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(itemList[i], (itemId) => { self.OnItemClick(itemId).Coroutine(); }).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);
            }

            for (int i = itemList.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].GameObject.SetActive(false);
            }
        }

        private static async ETTask OnItemClick(this UIHeroComponent self, long itemId)
        {
            UI uI = await self.Root().GetComponent<UIComponent>().Create(UIType.UIItemTip);
            if (uI != null)
            {
                uI.GetComponent<UIItemTipComponent>().UpdateInfo(new UIItemTipData()
                {
                    ItemId = itemId,
                    UIItemTipOpType = UIItemTipOpType.UIHero_Wear,
                    HeroId = self.CurrentHeroId
                });
            }
        }

        public static void ShowItemList(this UIHeroComponent self)
        {
            self.UIHeroInfo_2.SetActive(false);
            self.ScrollView_ItemList.SetActive(true);
            self.UpdateItemList();
        }

        private static async ETTask OnButton_ShengJi(this UIHeroComponent self)
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