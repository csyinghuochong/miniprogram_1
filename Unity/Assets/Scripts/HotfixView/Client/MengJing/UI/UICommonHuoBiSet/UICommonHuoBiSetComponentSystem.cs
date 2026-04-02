using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class DataUpdate_UpdateUserData_UICommonHuoBiSetComponentRefresh : AEvent<Scene, UpdateUserData>
    {
        protected override async ETTask Run(Scene scene, UpdateUserData args)
        {
            if (args.UserDataType != UserDataType.Gold && args.UserDataType != UserDataType.Diamond)
            {
                return;
            }

            foreach (UICommonHuoBiSetComponent ui in UICommonHuoBiSetComponent.InstanceList)
            {
                if (args.UserDataType == UserDataType.Gold)
                {
                    ui.UpdateGold();
                }

                if (args.UserDataType == UserDataType.Diamond)
                {
                    ui.UpdateDiamond();
                }
            }

            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.Demo)]
    public class InventoryUpdate_UICommonHuoBiSetRefresh : AEvent<Scene, InventoryUpdate>
    {
        protected override async ETTask Run(Scene scene, InventoryUpdate args)
        {
            foreach (UICommonHuoBiSetComponent ui in UICommonHuoBiSetComponent.InstanceList)
            {
                ui.UpdateLotteryTicket();
            }

            await ETTask.CompletedTask;
        }
    }

    [FriendOf(typeof(UICommonHuoBiSetComponent))]
    [EntitySystemOf(typeof(UICommonHuoBiSetComponent))]
    public static partial class UICommonHuoBiSetComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UICommonHuoBiSetComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = self.GameObject.GetComponent<ReferenceCollector>();

            self.LotteryTicket = rc.Get<GameObject>("LotteryTicket");
            self.LotteryTicket.SetActive(false);
            self.Text_Type_LotteryTicket = rc.Get<GameObject>("Text_Type_LotteryTicket").GetComponent<TMP_Text>();
            self.Gold = rc.Get<GameObject>("Gold");
            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Button_AddGold = rc.Get<GameObject>("Button_AddGold").GetComponent<Button>();
            self.Diamond = rc.Get<GameObject>("Diamond");
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
            self.Button_AddDiamond = rc.Get<GameObject>("Button_AddDiamond").GetComponent<Button>();

            self.Button_AddGold.onClick.AddListener(() => { Log.Warning("弹出来金币界面"); });
            self.Button_AddDiamond.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIRecharge).Coroutine(); });

            UICommonHuoBiSetComponent.InstanceList.Add(self);

            self.UpdateLotteryTicket();
            self.UpdateGold();
            self.UpdateDiamond();
        }

        [EntitySystem]
        private static void Destroy(this UICommonHuoBiSetComponent self)
        {
            UICommonHuoBiSetComponent.InstanceList.Remove(self);
        }

        public static void UpdateLotteryTicket(this UICommonHuoBiSetComponent self)
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

        public static void UpdateGold(this UICommonHuoBiSetComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();

            if (userInfoComponent.Gold >= 100000)
            {
                self.Text_Type_Gold.SetTextFormat("{0}万", userInfoComponent.Gold / 10000);
                return;
            }

            self.Text_Type_Gold.SetText(userInfoComponent.Gold);
        }

        public static void UpdateDiamond(this UICommonHuoBiSetComponent self)
        {
            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();

            if (userInfoComponent.Diamond >= 100000)
            {
                self.Text_Type_Diamond.SetTextFormat("{0}万", userInfoComponent.Diamond / 10000);
                return;
            }

            self.Text_Type_Diamond.SetText(userInfoComponent.Diamond);
        }
    }
}