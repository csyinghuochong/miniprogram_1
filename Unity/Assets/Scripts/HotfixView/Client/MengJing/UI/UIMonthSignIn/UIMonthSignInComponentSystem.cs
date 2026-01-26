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

            for (int i = 0; i < rewardItems.Count; i++)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                newItem.UpdateInfo(rewardItems[i].ItemId, rewardItems[i].ItemNum).Coroutine();
            }
        }
    }
}