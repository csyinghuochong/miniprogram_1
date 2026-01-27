using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRewardItem))]
    [FriendOf(typeof(UIRewardItem))]
    public static partial class UIRewardItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIRewardItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_Required = rc.Get<GameObject>("Text_Required").GetComponent<TMP_Text>();
            self.Text_Progress = rc.Get<GameObject>("Text_Progress").GetComponent<TMP_Text>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.GameObject_NotCompleted = rc.Get<GameObject>("GameObject_NotCompleted");
            self.GameObject_Received = rc.Get<GameObject>("GameObject_Received");
            self.Button_GetReward = rc.Get<GameObject>("Button_GetReward").GetComponent<Button>();

            self.Button_GetReward.AddListener(() => { self.OnButton_GetReward().Coroutine(); });
        }

        public static void UpdateInfo(this UIRewardItem self, int rewardId)
        {
            self.RewardId = rewardId;

            ServerOpenRewardConfig serverOpenRewardConfig = ServerOpenRewardConfigCategory.Instance.Get(self.RewardId);

            if (serverOpenRewardConfig.RequiredType == 1)
            {
                self.Text_Required.SetTextFormat("等级达到{0}级",serverOpenRewardConfig.RequiredValue);
            }
            else if(serverOpenRewardConfig.RequiredType == 2)
            {
                self.Text_Required.SetTextFormat("战力达到{0}",serverOpenRewardConfig.RequiredValue);
            }

            // 道具奖励
            RewardItem[] rewardItems = serverOpenRewardConfig.RewardItem;

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

        private static async ETTask OnButton_GetReward(this UIRewardItem self)
        {
            
        }
    }
}