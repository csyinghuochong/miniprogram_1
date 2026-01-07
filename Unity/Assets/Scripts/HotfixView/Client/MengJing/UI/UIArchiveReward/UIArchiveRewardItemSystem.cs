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

            ArchiveRewardConfig archiveRewardConfig = ArchiveRewardConfigCategory.Instance.Get(self.RewardId);
            ArchiveComponentC archiveComponent = self.Root().GetComponent<ArchiveComponentC>();
            int currentPoint = archiveComponent.GetCurrentPoint();

            // 进度条
            self.Text_RewardPoints.SetText(archiveRewardConfig.RequiredPoints);
            int lastPoint = 0;
            foreach (ArchiveRewardConfig config in ArchiveRewardConfigCategory.Instance.DataList)
            {
                if (config.RequiredPoints < archiveRewardConfig.RequiredPoints)
                {
                    lastPoint = config.RequiredPoints;
                }
            }

            if (currentPoint >= archiveRewardConfig.RequiredPoints)
            {
                self.Image_PointsProgress.fillAmount = 1;
            }
            else if (currentPoint < lastPoint)
            {
                self.Image_PointsProgress.fillAmount = 0;
            }
            else
            {
                self.Image_PointsProgress.fillAmount = Mathf.Clamp01((currentPoint - lastPoint) / (float)(archiveRewardConfig.RequiredPoints - lastPoint));
            }

            bool received = archiveComponent.ReceivedArchiveRewardIds.Contains(self.RewardId);
            if (received)
            {
                // 已经领取奖励
            }

            // 道具奖励
            RewardItem[] rewardItems = archiveRewardConfig.RewardItem;

            while (self.UICommonItemList.Count < rewardItems.Length)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UICommonItemList.Add(newItem);
            }

            for (int i = 0; i < rewardItems.Length; i++)
            {
                self.UICommonItemList[i].UpdateInfo(rewardItems[i].ItemId, rewardItems[i].ItemNum).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);
            }

            for (int i = rewardItems.Length; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].GameObject.SetActive(false);
            }

            await ETTask.CompletedTask;
        }
    }
}