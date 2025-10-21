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

            self.UIHeroInfo = rc.Get<GameObject>("UIHeroInfo");
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Text_HeroCP = rc.Get<GameObject>("Text_HeroCP").GetComponent<TMP_Text>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Text_HeroLv = rc.Get<GameObject>("Text_HeroLv").GetComponent<TMP_Text>();
            self.Slider_HeroExp = rc.Get<GameObject>("Slider_HeroExp").GetComponent<Slider>();
            self.Text_HeroExp = rc.Get<GameObject>("Text_HeroExp").GetComponent<TMP_Text>();
            self.UIEquipmentItem_1 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_1"));
            self.UIEquipmentItem_2 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_2"));
            self.UIEquipmentItem_3 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_3"));
            self.UIEquipmentItem_4 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_4"));
            self.UIEquipmentItem_5 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_5"));
            self.UIEquipmentItem_6 = self.AddChild<UIEquipmentItem, GameObject>(rc.Get<GameObject>("UIEquipmentItem_6"));
            self.ScrollView_ItemList = rc.Get<GameObject>("ScrollView_ItemList");
            self.ScrollView_ItemList.SetActive(false);
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").GetComponent<Transform>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.UICommonItem.gameObject.SetActive(false);
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
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHero); });
            self.Button_Hero.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIHeroList).Coroutine(); });
            self.Button_Formation.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIFormation).Coroutine(); });

            self.UpdateHeroList();
            self.SelectFirstHero();
        }

        [EntitySystem]
        private static void Destroy(this UIHeroComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItemList = null;
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

        private static async ETTask UpdateHeroInfo(this UIHeroComponent self)
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
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            self.Text_HeroLv.SetTextFormat("等级：{0}", hero.Lv);
            int maxExp = 100; // 暂时
            self.Slider_HeroExp.value = hero.Exp * 1f / maxExp;
            self.Text_HeroExp.SetTextFormat("{0}/{1}", hero.Exp, maxExp);

            self.UIEquipmentItem_1.UpdateInfo(hero).Coroutine();
            self.UIEquipmentItem_2.UpdateInfo(hero).Coroutine();
            self.UIEquipmentItem_3.UpdateInfo(hero).Coroutine();
            self.UIEquipmentItem_4.UpdateInfo(hero).Coroutine();
            self.UIEquipmentItem_5.UpdateInfo(hero).Coroutine();
            self.UIEquipmentItem_6.UpdateInfo(hero).Coroutine();
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
            self.ScrollView_ItemList.SetActive(true);
            self.UpdateItemList();
        }
    }
}