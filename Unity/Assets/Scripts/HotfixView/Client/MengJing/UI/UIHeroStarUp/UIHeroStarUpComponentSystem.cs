using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroStarUpComponent))]
    [FriendOf(typeof(UIHeroStarUpComponent))]
    public static partial class UIHeroStarUpComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroStarUpComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Text_HeroLv = rc.Get<GameObject>("Text_HeroLv").GetComponent<TMP_Text>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Transform_HeroStar = rc.Get<GameObject>("Transform_HeroStar").transform;
            self.Slider_HeroHunShi = rc.Get<GameObject>("Slider_HeroHunShi").GetComponent<Slider>();
            self.Text_HeroHunShi = rc.Get<GameObject>("Text_HeroHunShi").GetComponent<TMP_Text>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.Text_Tip = rc.Get<GameObject>("Text_Tip").GetComponent<TMP_Text>();
            self.Button_Use_10 = rc.Get<GameObject>("Button_Use_10").GetComponent<Button>();
            self.Button_Use_1 = rc.Get<GameObject>("Button_Use_1").GetComponent<Button>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHeroStarUp); });
            self.Button_Use_10.AddListener(() => { self.OnButton_Use(10).Coroutine(); });
            self.Button_Use_1.AddListener(() => { self.OnButton_Use(1).Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIHeroStarUpComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItemList = null;
        }

        public static async ETTask UpdateInfo(this UIHeroStarUpComponent self, long heroId)
        {
            self.HeroId = heroId;

            Hero hero = self.Root().GetComponent<HeroComponentC>().GetHero(heroId);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_Tip.SetText("");
            self.Text_HeroName.SetText(heroConfig.HeroName);
            
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            
            int maxExp = heroConfig.HeroUpStarNeed[hero.Star];
            self.Slider_HeroHunShi.value = hero.HunShi * 1f / maxExp;
            self.Text_HeroHunShi.SetTextFormat("{0}/{1}", hero.HunShi, maxExp);

            UICommonHelper.HideChild(self.Transform_HeroStar.gameObject);
            for (int i = 0; i < heroConfig.HeroUpStarNeed.Length - 1; i++)
            {
                if (i < self.Transform_HeroStar.childCount)
                {
                    self.Transform_HeroStar.GetChild(i).gameObject.SetActive(true);
                    self.Transform_HeroStar.GetChild(i).GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    GameObject prefab = self.Transform_HeroStar.GetChild(0).gameObject;
                    GameObject go = UnityEngine.Object.Instantiate(prefab, self.Transform_HeroStar);
                    go.SetActive(true);
                }

                GameObject star = self.Transform_HeroStar.GetChild(i).GetChild(0).gameObject;
                star.SetActive(hero.Star > i);
            }

            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = inventoryComponentC.GetItemsBySubType(ItemSubType.HeroHunshi);

            while (self.UICommonItemList.Count < itemList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(itemList[i], (itemId) => { self.OnItemClick(itemId); }).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);
            }

            for (int i = itemList.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].GameObject.SetActive(false);
            }

            if (itemList.Count >= 1)
            {
                self.OnItemClick(itemList[0].Id);
            }

            await ETTask.CompletedTask;
        }

        private static void OnItemClick(this UIHeroStarUpComponent self, long itemId)
        {
            self.ItemId = itemId;

            foreach (UICommonItem uiCommonItem in self.UICommonItemList)
            {
                InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();
                Item item = inventoryComponentC.GetItem(self.ItemId);
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                self.Text_Tip.SetTextFormat("预计增加:{0}-{1}魂石", itemConfig.ItemUseParInt[0], itemConfig.ItemUseParInt[1]);

                uiCommonItem.SetSelected(self.ItemId);
            }
        }

        private static async ETTask OnButton_Use(this UIHeroStarUpComponent self, int num)
        {
            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(self.ItemId);
            if (item == null)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("请选择道具");
                return;
            }

            if (num > item.Num)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("道具数量不足");
                return;
            }

            int error = await ClientInventoryHelper.UseItem(self.Root(), self.ItemId, num, self.HeroId);

            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.UpdateInfo(self.HeroId).Coroutine();
        }
    }
}