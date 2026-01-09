using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(UILotteryDrawRewardPreviewComponent))]
    public static partial class UILotteryDrawRewardPreviewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawRewardPreviewComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            // DropConfig dropConfig = DropConfigCategory.Instance.Get(ConfigData.LotteryDrawDropId);
            // List<RewardItem> rewardItems = new List<RewardItem>();
            // for (int i = 0; i < dropConfig.DropItemInfos.Length; i++)
            // {
            //     RewardItem rewardItem = new RewardItem();
            //     rewardItem.ItemId = dropConfig.DropItemInfos[i].ItemId;
            //     rewardItem.ItemNum = dropConfig.DropItemInfos[i].MaxNum;
            //     rewardItems.Add(rewardItem);
            // }
        }

        [EntitySystem]
        private static void Destroy(this UILotteryDrawRewardPreviewComponent self)
        {
        }
    }
}