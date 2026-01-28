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
                self.Text_Required.SetTextFormat("等级达到{0}级", serverOpenRewardConfig.RequiredValue);
            }
            else if (serverOpenRewardConfig.RequiredType == 2)
            {
                self.Text_Required.SetTextFormat("战力达到{0}", serverOpenRewardConfig.RequiredValue);
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

            ActivityServerOpenComponentC activityServerOpenComponent = self.Root().GetComponent<ActivityServerOpenComponentC>();
            bool isReceived = activityServerOpenComponent.ReceivedServerOpenRewardIds.Contains(self.RewardId);
            bool isCompleted = false;
            int value = 0;
            if (serverOpenRewardConfig.RequiredType == 1)
            {
                // 等级要求
                UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
                value = userInfoComponent.Lv;
            }
            else
            {
                // 战力要求
                NumericComponentC numericComponent = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<NumericComponentC>();
                value = numericComponent.GetAsInt(NumericType.CombatPower);
            }

            isCompleted = value >= serverOpenRewardConfig.RequiredValue;
            self.Text_Progress.SetTextFormat("{0}/{1}", value > serverOpenRewardConfig.RequiredValue ? serverOpenRewardConfig.RequiredValue : value, serverOpenRewardConfig.RequiredValue);

            self.GameObject_NotCompleted.gameObject.SetActive(!isReceived && !isCompleted);
            self.GameObject_Received.gameObject.SetActive(isReceived);
            self.Button_GetReward.gameObject.SetActive(!isReceived && isCompleted);
        }

        private static async ETTask OnButton_GetReward(this UIRewardItem self)
        {
            int error = await ClientActivityHelper.ActivityServerOpenGetReward(self.Root(), self.RewardId);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("领取成功");
                self.UpdateInfo(self.RewardId);
            }
        }
    }
}