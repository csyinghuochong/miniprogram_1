using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIAchievementComponent))]
    [FriendOf(typeof(UIAchievementComponent))]
    public static partial class UIAchievementComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIAchievementComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Type_Hero = rc.Get<GameObject>("Button_Type_Hero").GetComponent<Button>();
            self.Content_UIAchievementItem = rc.Get<GameObject>("Content_UIAchievementItem").transform;
            self.UIAchievementItem = rc.Get<GameObject>("UIAchievementItem");
            self.Image_CurrentPoints = rc.Get<GameObject>("Image_CurrentPoints").GetComponent<Image>();
            self.Text_CurrentPoints = rc.Get<GameObject>("Text_CurrentPoints").GetComponent<TMP_Text>();
            self.Button_GetReward = rc.Get<GameObject>("Button_GetReward").GetComponent<Button>();
            self.GameObject_LookReward = rc.Get<GameObject>("GameObject_LookReward");
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.UIAchievementItem.SetActive(false);

            self.Button_Close.AddListener(() => self.Root().GetComponent<UIComponent>().Remove(UIType.UIAchievement));
            self.Button_GetReward.AddListener(() => { self.OnButton_GetReward().Coroutine(); });

            self.UpdateList();
            self.UpdatePoint();
        }

        [EntitySystem]
        private static void Destroy(this UIAchievementComponent self)
        {
            self.UIAchievementItemList.Clear();
            self.UIAchievementItem = null;
        }

        private static void UpdateList(this UIAchievementComponent self)
        {
            AchievementComponentC achievementComponent = self.Root().GetComponent<AchievementComponentC>();
            List<EntityRef<Achievement>> achievementList = achievementComponent.AchievementList;

            while (self.UIAchievementItemList.Count < achievementList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIAchievementItem, self.Content_UIAchievementItem);
                UIAchievementItem newItem = self.AddChild<UIAchievementItem, GameObject>(go);
                self.UIAchievementItemList.Add(newItem);
            }

            for (int i = 0; i < achievementList.Count; i++)
            {
                self.UIAchievementItemList[i].UpdateInfo(achievementList[i]);
                self.UIAchievementItemList[i].GameObject.SetActive(true);
            }

            for (int i = achievementList.Count; i < self.UIAchievementItemList.Count; i++)
            {
                self.UIAchievementItemList[i].GameObject.SetActive(false);
            }
        }

        private static void UpdatePoint(this UIAchievementComponent self)
        {
            AchievementComponentC achievementComponent = self.Root().GetComponent<AchievementComponentC>();

            int max = 0;
            AchievementRewardConfig nowConfig = null;
            foreach (AchievementRewardConfig achievementRewardConfig in AchievementRewardConfigCategory.Instance.DataList)
            {
                if (!achievementComponent.ReceivedAchievementRewardIds.Contains(achievementRewardConfig.Id))
                {
                    nowConfig = achievementRewardConfig;

                    break;
                }
            }

            if (nowConfig == null)
            {
                nowConfig = AchievementRewardConfigCategory.Instance.DataList[^1];
            }

            max = nowConfig.RequiredPoints;
            self.RewardId = nowConfig.Id;

            int point = achievementComponent.GetCurrentPoint();
            self.Text_CurrentPoints.SetTextFormat("{0}/{1}", point, max);
            self.Image_CurrentPoints.fillAmount = Mathf.Clamp01(point * 1f / max);

            // 道具奖励
            RewardItem[] rewardItems = nowConfig.RewardItem;

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
        }

        private static async ETTask OnButton_GetReward(this UIAchievementComponent self)
        {
            int error = await ClientAchievementHelper.ReceivedAchievementReward(self.Root(), self.RewardId);
            if (error == ErrorCode.ERR_Success)
            {
                self.UpdatePoint();
            }
        }
    }
}