using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMonthSignInComponent))]
    [FriendOf(typeof(UIMonthSignInComponent))]
    public static partial class UIMonthSignInComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMonthSignInComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_SignedInDays = rc.Get<GameObject>("Text_SignedInDays").GetComponent<TMP_Text>();
            self.Image_SignedInDays = rc.Get<GameObject>("Image_SignedInDays").GetComponent<Image>();
            self.Button_SignedInSeven = rc.Get<GameObject>("Button_SignedInSeven").GetComponent<Button>();
            self.Button_SignedInFourteen = rc.Get<GameObject>("Button_SignedInFourteen").GetComponent<Button>();
            self.Button_SignedInTwentyOne = rc.Get<GameObject>("Button_SignedInTwentyOne").GetComponent<Button>();
            self.Button_SignedInThirty = rc.Get<GameObject>("Button_SignedInThirty").GetComponent<Button>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Button_SignIn = rc.Get<GameObject>("Button_SignIn").GetComponent<Button>();

            self.Button_SignedInSeven.AddListener(() => { self.OnButton_SignedInTotal(10002001).Coroutine(); });
            self.Button_SignedInFourteen.AddListener(() => { self.OnButton_SignedInTotal(10002002).Coroutine(); });
            self.Button_SignedInTwentyOne.AddListener(() => { self.OnButton_SignedInTotal(10002003).Coroutine(); });
            self.Button_SignedInThirty.AddListener(() => { self.OnButton_SignedInTotal(10002004).Coroutine(); });
            self.Button_SignIn.AddListener(() => { self.OnButton_SignIn().Coroutine(); });
            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIMonthSignIn); });

            self.UpdateList();
        }

        [EntitySystem]
        private static void Destroy(this UIMonthSignInComponent self)
        {
            self.UICommonItemList.Clear();
            self.UICommonItem = null;
        }

        public static void UpdateList(this UIMonthSignInComponent self)
        {
            ActivityMonthSignInComponentC activityMonthSignInComponent = self.Root().GetComponent<ActivityMonthSignInComponentC>();

            self.Text_SignedInDays.SetTextFormat("累计\n{0}天", activityMonthSignInComponent.TotalSignInDay);

            List<MonthSignInConfig> monthSignInConfigs = MonthSignInConfigCategory.Instance.DataList;

            List<RewardItem> rewardItems = new List<RewardItem>();

            for (int i = 0; i < monthSignInConfigs.Count; i++)
            {
                RewardItem rewardItem = new RewardItem();

                if (monthSignInConfigs[i].SignInType == 1)
                {
                    rewardItem = monthSignInConfigs[i].RewardItem;
                    rewardItems.Add(rewardItem);
                }
            }

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
                self.UICommonItemList[i].Image_Selected.gameObject.SetActive(activityMonthSignInComponent.TotalSignInDay >= i + 1);
            }

            for (int i = rewardItems.Count; i < self.UICommonItemList.Count; i++)
            {
                self.UICommonItemList[i].UpdateInfo(null).Coroutine();
                self.UICommonItemList[i].GameObject.SetActive(true);
            }
        }

        private static async ETTask OnButton_SignIn(this UIMonthSignInComponent self)
        {
            int error = await ClientActivityHelper.ActivityMonthSignIn(self.Root());
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("签到成功！");
                self.UpdateList();
            }
        }

        private static async ETTask OnButton_SignedInTotal(this UIMonthSignInComponent self, int configId)
        {
            int error = await ClientActivityHelper.ActivityMonthSignInTotal(self.Root(), configId);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("领取累计签到奖励成功！");
            }
        }
    }
}