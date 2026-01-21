using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRecycleComponent))]
    [FriendOf(typeof(UIRecycleComponent))]
    public static partial class UIRecycleComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIRecycleComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Type_Bag = rc.Get<GameObject>("Button_Type_Bag").GetComponent<Button>();
            self.Button_Type_Hero = rc.Get<GameObject>("Button_Type_Hero").GetComponent<Button>();
            self.GameObject_Bag = rc.Get<GameObject>("GameObject_Bag");
            self.GameObject_Bag.SetActive(false);
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.GameObject_Hero = rc.Get<GameObject>("GameObject_Hero");
            self.GameObject_Hero.SetActive(false);
            self.Content_UIHeroRecycleItem = rc.Get<GameObject>("Content_UIHeroRecycleItem").transform;
            self.UIHeroRecycleItem = rc.Get<GameObject>("UIHeroRecycleItem");
            self.UIHeroRecycleItem.gameObject.SetActive(false);
            self.Content_LookReward = rc.Get<GameObject>("Content_LookReward").transform;
            self.Button_Recycle = rc.Get<GameObject>("Button_Recycle").GetComponent<Button>();

            self.Button_Type_Bag.onClick.AddListener(() => { self.SetShowType(0); });
            self.Button_Type_Hero.onClick.AddListener(() => { self.SetShowType(1); });
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIRecycle); });

            self.SetShowType(0);
        }

        [EntitySystem]
        private static void Destroy(this UIRecycleComponent self)
        {
            self.UICommonItemList.Clear();
            self.UILookRewardList.Clear();
            self.UIHeroRecycleItemList.Clear();
            self.UICommonItem = null;
            self.UIHeroRecycleItem = null;
        }

        private static void SetShowType(this UIRecycleComponent self, int page)
        {
            self.CurrentPage = page;
            self.Button_Type_Bag.transform.Find("Image_On").gameObject.SetActive(page == 0);
            self.Button_Type_Bag.transform.Find("Image_Off").gameObject.SetActive(page != 0);
            self.Button_Type_Hero.transform.Find("Image_On").gameObject.SetActive(page == 1);
            self.Button_Type_Hero.transform.Find("Image_Off").gameObject.SetActive(page != 1);

            self.UpdateItemList(page);
        }

        public static void UpdateItemList(this UIRecycleComponent self, int page)
        {
            List<Item> itemList = null;
            List<Hero> heroList = null;

            if (page == 0)
            {
                InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();
                itemList = inventoryComponentC.GetItemsByContainer(InventoryContainerType.Bag);
                self.UpdateBagList(itemList);
                self.UpdateLookRewardList(CommonHelp.GetRecycleItems(self.SelectItemList));
            }
            else if (page == 1)
            {
                HeroComponentC heroComponentC = self.Root().GetComponent<HeroComponentC>();
                heroList = heroComponentC.GetAllHero();
                self.UpdateHeroList(heroList);
                self.UpdateLookRewardList(CommonHelp.GetRecycleItems(self.SelectHeroList));
            }
            else
            {
                return;
            }
        }

        public static void UpdateBagList(this UIRecycleComponent self, List<Item> itemList)
        {
            while (self.UICommonItemList.Count < (itemList.Count > 100 ? itemList.Count : 100))
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(itemList[i], (item) => { self.SelectItem(item); }).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);

                bool selected = false;
                foreach (Item item in self.SelectItemList)
                {
                    if (item == itemList[i])
                    {
                        selected = true;
                        break;
                    }
                }

                self.UICommonItemList[i].Image_Selected.gameObject.SetActive(selected);
            }

            for (int i = itemList.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(null).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);
            }

            self.GameObject_Bag.SetActive(true);
            self.GameObject_Hero.SetActive(false);
        }

        public static void UpdateHeroList(this UIRecycleComponent self, List<Hero> heroList)
        {
            while (self.UIHeroRecycleItemList.Count < (heroList.Count > 100 ? heroList.Count : 100))
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIHeroRecycleItem, self.Content_UIHeroRecycleItem);
                UIHeroRecycleItem newItem = self.AddChild<UIHeroRecycleItem, GameObject>(go);
                self.UIHeroRecycleItemList.Add(newItem);
            }

            for (int i = 0; i < heroList.Count; i++)
            {
                self.UIHeroRecycleItemList[i].UpdateInfo(heroList[i]).Coroutine();
                self.UIHeroRecycleItemList[i].GameObject.SetActive(true);

                bool selected = false;
                foreach (Hero hero in self.SelectHeroList)
                {
                    if (hero == heroList[i])
                    {
                        selected = true;
                        break;
                    }
                }

                self.UIHeroRecycleItemList[i].Image_Selected.gameObject.SetActive(selected);
            }

            for (int i = heroList.Count; i < self.UIHeroRecycleItemList.Count; i++)
            {
                self.UIHeroRecycleItemList[i].UpdateInfo(null).Coroutine();
                self.UIHeroRecycleItemList[i].GameObject.SetActive(true);
            }

            self.GameObject_Hero.SetActive(true);
            self.GameObject_Bag.SetActive(false);
        }

        public static void UpdateLookRewardList(this UIRecycleComponent self, List<RewardItem> rewardItemList)
        {
            while (self.UILookRewardList.Count < rewardItemList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_LookReward);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UILookRewardList.Add(newItem);
            }

            for (int i = 0; i < rewardItemList.Count; i++)
            {
                self.UILookRewardList[i].UpdateInfo(rewardItemList[i].ItemId, rewardItemList[i].ItemNum).Coroutine();
                self.UILookRewardList[i].GameObject.SetActive(true);
            }

            for (int i = rewardItemList.Count; i < self.UILookRewardList.Count; i++)
            {
                self.UILookRewardList[i].GameObject.SetActive(false);
            }
        }

        private static void SelectItem(this UIRecycleComponent self, Item item)
        {
            bool selected = false;
            for (int i = 0; i < self.SelectItemList.Count; i++)
            {
                Item old = self.SelectItemList[i];
                if (old == item)
                {
                    selected = true;
                    break;
                }
            }

            if (selected)
            {
                self.SelectItemList.Remove(item);
            }
            else
            {
                self.SelectItemList.Add(item);
            }

            self.UpdateItemList(0);
        }

        public static void SelectHero(this UIRecycleComponent self, Hero hero)
        {
            bool selected = false;
            for (int i = 0; i < self.SelectHeroList.Count; i++)
            {
                Hero old = self.SelectHeroList[i];
                if (old == hero)
                {
                    selected = true;
                    break;
                }
            }

            if (selected)
            {
                self.SelectHeroList.Remove(hero);
            }
            else
            {
                self.SelectHeroList.Add(hero);
            }

            self.UpdateItemList(1);
        }
    }
}