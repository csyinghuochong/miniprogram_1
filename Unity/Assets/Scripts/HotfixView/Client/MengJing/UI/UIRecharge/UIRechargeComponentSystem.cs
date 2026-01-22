
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
            self.Button_Reward = rc.Get<GameObject>("Button_Reward").GetComponent<Button>();


            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIRecharge); });
            self.Button_RechargeOne.AddListener(() => { self.OnRecharge(6); });
            self.Button_RechargeTwo.AddListener(() => { self.OnRecharge(30); });
            self.Button_RechargeThree.AddListener(() => { self.OnRecharge(98); });
            self.Button_RechargeFour.AddListener(() => { self.OnRecharge(128); });
            self.Button_RechargeFive.AddListener(() => { self.OnRecharge(198); });
            self.Button_RechargeSix.AddListener(() => { self.OnRecharge(328); });
        }

        public static void OnRecharge(this UIRechargeComponent self,int num)
        {
            Log.Warning("我充值了" + num + "元");
        }
        

    }
}