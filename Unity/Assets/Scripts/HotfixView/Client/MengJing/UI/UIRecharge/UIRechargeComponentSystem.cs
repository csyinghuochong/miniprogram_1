using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRechargeComponent))]
    [FriendOf(typeof(UIRechargeComponent))]
    public static partial class UIRechargeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIRechargeComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_RechargeOne = rc.Get<GameObject>("Button_RechargeOne").GetComponent<Button>();
            self.Button_RechargeTwo = rc.Get<GameObject>("Button_RechargeTwo").GetComponent<Button>();
            self.Button_RechargeThree = rc.Get<GameObject>("Button_RechargeThree").GetComponent<Button>();
            self.Button_RechargeFour = rc.Get<GameObject>("Button_RechargeFour").GetComponent<Button>();
            self.Button_RechargeFive = rc.Get<GameObject>("Button_RechargeFive").GetComponent<Button>();
            self.Button_RechargeSix = rc.Get<GameObject>("Button_RechargeSix").GetComponent<Button>();
            self.Text_VipLv = rc.Get<GameObject>("Text_VipLv").GetComponent<TMP_Text>();
            self.Image_PointsProgress = rc.Get<GameObject>("Image_PointsProgress").GetComponent<Image>();
            self.Text_Points = rc.Get<GameObject>("Text_Points").GetComponent<TMP_Text>();
            self.Button_Reward = rc.Get<GameObject>("Button_Reward").GetComponent<Button>();
            self.UIRechargePointsRewardComponent = self.AddComponent<UIRechargePointsRewardComponent, GameObject>(rc.Get<GameObject>("GameObject_RechargePointsReward"));
            self.UIRechargePointsRewardComponent.GameObject.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIRecharge); });
            self.Button_RechargeOne.AddListener(() => { self.OnRecharge(6).Coroutine(); });
            self.Button_RechargeTwo.AddListener(() => { self.OnRecharge(30).Coroutine(); });
            self.Button_RechargeThree.AddListener(() => { self.OnRecharge(98).Coroutine(); });
            self.Button_RechargeFour.AddListener(() => { self.OnRecharge(128).Coroutine(); });
            self.Button_RechargeFive.AddListener(() => { self.OnRecharge(198).Coroutine(); });
            self.Button_RechargeSix.AddListener(() => { self.OnRecharge(328).Coroutine(); });
            self.Button_Reward.AddListener(() =>
            {
                self.UIRechargePointsRewardComponent.GameObject.SetActive(true);
                self.UIRechargePointsRewardComponent.UpdateInfo();
            });
            
            self.UpdateInfo();
        }

        public static void UpdateInfo(this UIRechargeComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            ActivityRechargePointComponentC activityRechargePointComponent = self.Root().GetComponent<ActivityRechargePointComponentC>();
            RechargePointsRewardConfig config = RechargePointsRewardConfigCategory.Instance.DataList[userInfoComponent.VipLv - 1];

            self.Text_VipLv.SetTextFormat("vip{0}", userInfoComponent.VipLv);
            self.Image_PointsProgress.fillAmount = Mathf.Clamp01((float)activityRechargePointComponent.RechargePoint / config.RequiredPoints);
            self.Text_Points.SetTextFormat("{0}/{1}", activityRechargePointComponent.RechargePoint, config.RequiredPoints);
        }

        public static async ETTask OnRecharge(this UIRechargeComponent self, int num)
        {
            int configId = 0;
            foreach (RechargeConfig rechargeConfig in RechargeConfigCategory.Instance.DataMap.Values)
            {
                if (rechargeConfig.Price == num)
                {
                    configId = rechargeConfig.Id;
                    break;
                }
            }

            int error = await ClientUserInfoHelper.Recharge(self.Root(), configId);

            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText(ZString.Format("充值{0}元成功", num));
                self.UpdateInfo();
            }
        }
    }
}