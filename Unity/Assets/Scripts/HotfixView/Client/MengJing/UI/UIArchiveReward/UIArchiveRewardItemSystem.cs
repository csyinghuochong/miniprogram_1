using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIArchiveRewardItem))]
    [FriendOf(typeof(UIArchiveRewardItem))]
    public static partial class UIArchiveRewardItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIArchiveRewardItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Image_PointsProgress = rc.Get<GameObject>("Image_PointsProgress").GetComponent<Image>();
            self.Text_RewardPoints = rc.Get<GameObject>("Text_RewardPoints").GetComponent<TMP_Text>();
            self.Button_GetReward = rc.Get<GameObject>("Button_GetReward").GetComponent<Button>();
        }

        public static async ETTask UpdateInfo(this UIArchiveRewardItem self, int rewardId)
        {
            self.RewardId = rewardId;

            ArchiveComponentC archiveComponent = self.Root().GetComponent<ArchiveComponentC>();
            int currentScore = archiveComponent.GetCurrentScore();

            // 进度条
            self.Text_RewardPoints.SetText(self.RewardId);
            int lastId = 0;
            foreach (int key in ConfigData.ArchiveRewardDic.Keys)
            {
                if (key < self.RewardId)
                {
                    lastId = key;
                }
            }
            self.Image_PointsProgress.fillAmount = (currentScore - lastId) / (float)(self.RewardId - lastId);

            bool received = archiveComponent.ReceivedArchiveRewardIds.Contains(self.RewardId);
            if (received)
            {
                // 已经领取奖励
            }
            
            // 道具奖励
            List<RewardItem> rewardItems = ConfigData.ArchiveRewardDic[self.RewardId];

            while (self.UICommonItemList.Count < rewardItems.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            for (int i = 0; i < rewardItems.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(rewardItems[i].ItemId, rewardItems[i].ItemNum).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);
            }

            for (int i = rewardItems.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].GameObject.SetActive(false);
            }

            await ETTask.CompletedTask;
        }
    }
}