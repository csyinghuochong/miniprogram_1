using System;
using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class DataUpdate_UpdateUserData_UILotteryDrawRefresh : AEvent<Scene, UpdateUserData>
    {
        protected override async ETTask Run(Scene scene, UpdateUserData args)
        {
            if (args.UserDataType != UserDataType.Diamond)
            {
                return;
            }

            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UILotteryDraw);
            if (ui == null)
            {
                return;
            }

            UILotteryDrawComponent uiLotteryDrawComponent = ui.GetComponent<UILotteryDrawComponent>();

            if (args.UserDataType == UserDataType.Diamond)
            {
                uiLotteryDrawComponent.UpdateDiamond();
            }

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class InventoryUpdate_UILotteryDrawRefresh : AEvent<Scene, InventoryUpdate>
    {
        protected override async ETTask Run(Scene scene, InventoryUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UILotteryDraw);
            if (ui == null)
            {
                return;
            }

            UILotteryDrawComponent uiLotteryDrawComponent = ui.GetComponent<UILotteryDrawComponent>();
            uiLotteryDrawComponent.UpdateLotteryTicket();

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UILotteryDrawComponent))]
    [FriendOf(typeof(UILotteryDrawComponent))]
    public static partial class UILotteryDrawComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UILotteryDrawComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_Type_LotteryTicket = rc.Get<GameObject>("Text_Type_LotteryTicket").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
            self.Button_AddDiamond = rc.Get<GameObject>("Button_AddDiamond").GetComponent<Button>();
            self.Button_RewardPreview = rc.Get<GameObject>("Button_RewardPreview").GetComponent<Button>();
            self.Button_Probability = rc.Get<GameObject>("Button_Probability").GetComponent<Button>();
            self.Button_Wish = rc.Get<GameObject>("Button_Wish").GetComponent<Button>();
            self.Text_BaoDiTips = rc.Get<GameObject>("Text_BaoDiTips").GetComponent<TMP_Text>();
            self.Button_DrawOne = rc.Get<GameObject>("Button_DrawOne").GetComponent<Button>();
            self.Button_DrawTen = rc.Get<GameObject>("Button_DrawTen").GetComponent<Button>();
            self.Text_FreeTime = rc.Get<GameObject>("Text_FreeTime").GetComponent<TMP_Text>();
            self.Toggle_SkipAnimation = rc.Get<GameObject>("Toggle_SkipAnimation").GetComponent<Toggle>();
            self.Image_WishIcon = rc.Get<GameObject>("Image_WishIcon").GetComponent<Image>();
            self.Image_WishIcon.gameObject.SetActive(false);
            self.UILotteryDrawRewardPreviewComponent =
                    self.AddComponent<UILotteryDrawRewardPreviewComponent, GameObject>(rc.Get<GameObject>("GameObject_RewardPreview"));
            self.UILotteryDrawProbabilityComponent =
                    self.AddComponent<UILotteryDrawProbabilityComponent, GameObject>(rc.Get<GameObject>("GameObject_Probability"));
            self.UILotteryDrawWishComponent = self.AddComponent<UILotteryDrawWishComponent, GameObject>(rc.Get<GameObject>("GameObject_Wish"));
            self.UILotteryDrawRewardPreviewComponent.GameObject.SetActive(false);
            self.UILotteryDrawProbabilityComponent.GameObject.SetActive(false);
            self.UILotteryDrawWishComponent.GameObject.SetActive(false);

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UILotteryDraw); });
            self.Button_AddDiamond.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIRecharge).Coroutine(); });
            self.Button_RewardPreview.AddListener(() => { self.UILotteryDrawRewardPreviewComponent.GameObject.SetActive(true); });
            self.Button_Probability.AddListener(() => { self.UILotteryDrawProbabilityComponent.GameObject.SetActive(true); });
            self.Button_Wish.AddListener(() => { self.UILotteryDrawWishComponent.GameObject.SetActive(true); });
            self.Button_DrawOne.AddListener(() => { self.OnButton_Draw(0).Coroutine(); });
            self.Button_DrawTen.AddListener(() => { self.OnButton_Draw(1).Coroutine(); });

            self.UpdateBaoDiTip();
            self.UpdateFreeTime().Coroutine();
            self.UpdateDiamond();
            self.UpdateLotteryTicket();
            self.UpdateWishIcon().Coroutine();
        }

        private static void UpdateBaoDiTip(this UILotteryDrawComponent self)
        {
            NumericComponentC numericComponent = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<NumericComponentC>();
            self.Text_BaoDiTips.SetTextFormat("再抽取{0}次必得传说英雄", ConfigData.LotteryDrawBaoDi - numericComponent.GetAsInt(NumericType.LotteryDrawNum));
        }

        private static async ETTask UpdateFreeTime(this UILotteryDrawComponent self)
        {
            NumericComponentC numericComponent = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<NumericComponentC>();
            while (true)
            {
                if (self.IsDisposed)
                {
                    return;
                }

                long freeTime = numericComponent.GetAsLong(NumericType.LotteryDrawFreeTime);
                long nowTime = TimeHelper.ServerNow();
                if (nowTime > freeTime)
                {
                    self.Text_FreeTime.SetText("免费");
                }
                else
                {
                    DateTime endTime = TimeInfo.Instance.ToDateTime(freeTime);
                    DateTime time = TimeInfo.Instance.ToDateTime(TimeHelper.ServerNow());
                    TimeSpan timeSpan = endTime - time;

                    self.Text_FreeTime.SetText("{0}:{1}:{2}后免费", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
                }

                await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            }
        }

        private static async ETTask OnButton_Draw(this UILotteryDrawComponent self, int opeType)
        {
            M2C_LotteryDrawRequest response = await ClientLotteryDrawHelper.LotteryDrawRequest(self.Root(), opeType, self.WishItemId);

            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.UpdateBaoDiTip();

            // 奖励提示
            List<RewardItem> rewardItems = new List<RewardItem>();
            for (int i = 0; i < response.ItemInfoList.Count; i++)
            {
                RewardItem rewardItem = new RewardItem();
                rewardItem.ItemId = response.ItemInfoList[i].ConfigId;
                rewardItem.ItemNum = response.ItemInfoList[i].Num;
                rewardItems.Add(rewardItem);
            }

            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIGetReward);
            UIGetRewardComponent uiGetRewardComponent = ui.GetComponent<UIGetRewardComponent>();
            uiGetRewardComponent.OnInit(rewardItems);
        }

        public static void UpdateLotteryTicket(this UILotteryDrawComponent self)
        {
            InventoryComponentC inventoryComponentC = self.Root().GetComponent<InventoryComponentC>();

            List<Item> itemList = inventoryComponentC.GetItemsBySubType(ItemSubType.Type_6, InventoryContainerType.Bag);

            int ticket = 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                ticket += itemList[i].Num;
            }

            self.Text_Type_LotteryTicket.SetText(ticket);
        }

        public static void UpdateDiamond(this UILotteryDrawComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();

            if (userInfoComponent.Diamond >= 10000)
            {
                self.Text_Type_Diamond.SetTextFormat("{0}k", userInfoComponent.Diamond / 1000);
                return;
            }

            self.Text_Type_Diamond.SetText(userInfoComponent.Diamond);
        }

        public static async ETTask UpdateWishIcon(this UILotteryDrawComponent self)
        {
            
            if (self.WishItemId == 0)
            {
                return;
            }

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.WishItemId);

            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_WishIcon.overrideSprite = await self.Root().GetComponent<ResourcesLoaderComponent>().LoadAssetAsync<Sprite>(path);
            self.Image_WishIcon.gameObject.SetActive(true);
        }
    }
}