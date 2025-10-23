using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIHeroLvUpComponent))]
    [FriendOf(typeof(UIHeroLvUpComponent))]
    public static partial class UIHeroLvUpComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIHeroLvUpComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_HeroName = rc.Get<GameObject>("Text_HeroName").GetComponent<TMP_Text>();
            self.Text_HeroLv = rc.Get<GameObject>("Text_HeroLv").GetComponent<TMP_Text>();
            self.Image_HeroIcon = rc.Get<GameObject>("Image_HeroIcon").GetComponent<Image>();
            self.Slider_HeroExp = rc.Get<GameObject>("Slider_HeroExp").GetComponent<Slider>();
            self.Text_HeroExp = rc.Get<GameObject>("Text_HeroExp").GetComponent<TMP_Text>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.Text_Tip = rc.Get<GameObject>("Text_Tip").GetComponent<TMP_Text>();
            self.Button_Use_10 = rc.Get<GameObject>("Button_Use_10").GetComponent<Button>();
            self.Button_Use_1 = rc.Get<GameObject>("Button_Use_1").GetComponent<Button>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIHeroLvUp); });
            self.Button_Use_10.AddListener(() => { self.OnButton_Use(10).Coroutine(); });
            self.Button_Use_1.AddListener(() => { self.OnButton_Use(1).Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIHeroLvUpComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItemList = null;
        }

        public static async ETTask UpdateInfo(this UIHeroLvUpComponent self, long heroId)
        {
            self.HeroId = heroId;

            Hero hero = self.Root().GetComponent<HeroComponentC>().GetHero(heroId);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(hero.ConfigId);
            self.Text_HeroName.SetText(heroConfig.HeroName);
            self.Text_HeroLv.SetTextFormat("等级：{0}", hero.Lv);
            
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.HeroIcon, heroConfig.HeroHeadIcon);
            self.Image_HeroIcon.sprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            
            int maxExp = ExpConfigCategory.Instance.Get(hero.Lv).HeroUpExp;
            self.Slider_HeroExp.value = hero.Exp * 1f / maxExp;
            self.Text_HeroExp.SetTextFormat("{0}/{1}", hero.Exp, maxExp);

            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = inventoryComponentC.GetItemsBySubType(ItemType.Consume, (int)ItemConsumeType.HeroExp, InventoryContainerType.Bag);

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

            self.OnItemClick(0);

            await ETTask.CompletedTask;
        }

        private static void OnItemClick(this UIHeroLvUpComponent self, long itemId)
        {
            self.ItemId = itemId;

            foreach (UICommonItem uiCommonItem in self.UICommonItemList)
            {
                uiCommonItem.SetSelected(self.ItemId);
            }
        }

        private static async ETTask OnButton_Use(this UIHeroLvUpComponent self, int num)
        {
            Item item = self.Root().GetComponent<InventoryComponentC>().GetItem(self.ItemId);
            if (item == null)
            {
                Log.Warning("请选择道具");
                return;
            }

            if (num > item.Num)
            {
                Log.Warning("道具数量不足");
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