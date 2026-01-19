using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawWishComponent))]
    [FriendOf(typeof(UILotteryDrawWishComponent))]
    [FriendOfAttribute(typeof(ET.Client.UILotteryDrawComponent))]
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

            self.UpdateWishItemList();
            //ConfigData.LotteryDrawWishItemIdList
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawWishComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItem = null;
        }

        private static void UpdateWishItemList(this UILotteryDrawWishComponent self)
        {
            List<Item> items = new List<Item>();

            DropConfig dropConfig = DropConfigCategory.Instance.Get(ConfigData.LotteryDrawDropId);

            for (int i = 0; i < ConfigData.LotteryDrawWishItemIdList.Count; i++)
            {
                Item item = new Item();
                item.ConfigId = ConfigData.LotteryDrawWishItemIdList[i];
                
                for (int j = 0; j < dropConfig.DropItemInfos.Length; j++)
                {
                    if (dropConfig.DropItemInfos[j].ItemId == item.ConfigId)
                    {
                        item.Num = dropConfig.DropItemInfos[j].MaxNum;
                        break;
                    }
                }

                items.Add(item);
            }
            
        }

        private static void OnItemClick(this UILotteryDrawWishComponent self, Item item)
        {

        }
    }
}