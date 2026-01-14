using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawRewardPreviewComponent))]
    [FriendOf(typeof(UILotteryDrawRewardPreviewComponent))]
    public static partial class UILotteryDrawRewardPreviewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawRewardPreviewComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").GetComponent<Transform>();
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.GameObject.SetActive(false); });
            
            self.UpdateItemList();
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawRewardPreviewComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItem = null;
        }

        private static void UpdateItemList(this UILotteryDrawRewardPreviewComponent self)
        {
            DropConfig dropConfig = DropConfigCategory.Instance.Get(ConfigData.LotteryDrawDropId);
            List<RewardItem> rewardItems = new List<RewardItem>();
            for (int i = 0; i < dropConfig.DropItemInfos.Length; i++)
            {
                RewardItem rewardItem = new RewardItem();
                rewardItem.ItemId = dropConfig.DropItemInfos[i].ItemId;
                rewardItem.ItemNum = dropConfig.DropItemInfos[i].MaxNum;
                rewardItems.Add(rewardItem);
            }
            
            for (int i = 0; i < rewardItems.Count; i++)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                newItem.UpdateInfo(rewardItems[i].ItemId, rewardItems[i].ItemNum).Coroutine();
            }
        }
    }
}