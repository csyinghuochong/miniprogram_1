using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRechargePointsRewardItem))]
    [FriendOf(typeof(UIRechargePointsRewardItem))]
    public static partial class UIRechargePointsRewardItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIRechargePointsRewardItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Text_RequiredPoints = rc.Get<GameObject>("Text_RequiredPoints").GetComponent<TMP_Text>();
            self.GameObject_Received = rc.Get<GameObject>("GameObject_Received");
            self.GameObject_Received.SetActive(false);
            self.Button_GetReward = rc.Get<GameObject>("Button_GetReward").GetComponent<Button>();

            self.Button_GetReward.AddListener(() => { self.OnButton_GetReward().Coroutine(); });
        }

        public static void UpdateInfo(this UIRechargePointsRewardItem self, int rewardId)
        {
            self.RewardId = rewardId;

            RechargePointsRewardConfig rechargePointsRewardConfig = RechargePointsRewardConfigCategory.Instance.Get(self.RewardId);

            self.Text_RequiredPoints.SetTextFormat("累计获得\n{0}积分", rechargePointsRewardConfig.RequiredPoints);

            // 道具奖励
            RewardItem[] rewardItems = rechargePointsRewardConfig.RewardItem;

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
            
            ActivityRechargePointComponentC activityRechargePointComponentC = self.Root().GetComponent<ActivityRechargePointComponentC>();
            bool received = activityRechargePointComponentC.ReceivedRechargePointRewardIds.Contains(self.RewardId);
            if (received)
            {
                // 已经领取奖励
                self.GameObject_Received.SetActive(true);
                self.Button_GetReward.gameObject.SetActive(false);
            }
            
        }

        private static async ETTask OnButton_GetReward(this UIRechargePointsRewardItem self)
        {
            int error = await ClientActivityHelper.ActivityRechargePointGetReward(self.Root(), self.RewardId);
            
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("领取成功");
            }
        }
    }
}