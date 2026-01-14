using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawWishComponent))]
    [FriendOf(typeof(UILotteryDrawWishComponent))]
    public static partial class UILotteryDrawWishComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawWishComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.GameObject.SetActive(false); });

            self.UpdateItemList();
            //ConfigData.LotteryDrawWishItemIdList
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawWishComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItem = null;
        }

        private static void UpdateItemList(this UILotteryDrawWishComponent self)
        {
            List<Item> items = new List<Item>();

            for (int i = 0; i < ConfigData.LotteryDrawWishItemIdList.Count; i++)
            {
                Item item = new Item();
                item.ConfigId = ConfigData.LotteryDrawWishItemIdList[i];
                items.Add(item);
            }

            for (int i = 0; i < items.Count; i++)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                newItem.UpdateInfo(items[i], item => { Log.Warning("我点击了"); }).Coroutine();
            }
        }
    }
}