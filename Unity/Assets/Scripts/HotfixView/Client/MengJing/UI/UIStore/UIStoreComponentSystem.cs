using System;
using Cysharp.Text;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIStoreComponent))]
    [FriendOf(typeof(UIStoreComponent))]
    public static partial class UIStoreComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIStoreComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_RefreshTime = rc.Get<GameObject>("Text_RefreshTime").GetComponent<TMP_Text>();
            self.Button_RefreshTime = rc.Get<GameObject>("Button_Refresh").GetComponent<Button>();
            self.Content_UIStoreITem = rc.Get<GameObject>("Content_UIStoreItem").transform;
            self.UIStoreItem = rc.Get<GameObject>("UIStoreItem");
            self.UIStoreItem.SetActive(false);
            self.Dotween_Close = rc.Get<GameObject>("Dotween_Close").transform;
            self.Dotween_Scroll = rc.Get<GameObject>("Dotween_Scroll").transform;

            self.AddComponent<UICommonHuoBiSetComponent, GameObject>(rc.Get<GameObject>("UICommonHuoBiSet"));
            self.Button_Close.AddListener(() => { self.OnClose(); });
            self.Button_RefreshTime.AddListener(() => { self.OnButton_StoreRefresh().Coroutine(); });

            self.GetStoreInfo().Coroutine();
        }

        [EntitySystem]
        private static void Destroy(this UIStoreComponent self)
        {
            self.UIStoreItemList.Clear();
            self.UIStoreItem = null;
        }

        private static async ETTask GetStoreInfo(this UIStoreComponent self)
        {
            M2C_GetStoreInfo response = await ClientStoreHelper.GetStoreInfo(self.Root());
            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.RefreshTime = response.RefreshTime;
            self.StoreItemList = response.StoreItemList;
            self.StoreRefreshNum = response.RefreshNum;

            self.UpdateRefreshTime().Coroutine();
            self.UpdateStoreList();
        }

        private static async ETTask UpdateRefreshTime(this UIStoreComponent self)
        {
            DateTime endTime = TimeInfo.Instance.ToDateTime(self.RefreshTime);

            while (true)
            {
                if (self.IsDisposed)
                {
                    return;
                }

                DateTime time = TimeInfo.Instance.ToDateTime(TimeHelper.ServerNow());
                TimeSpan timeSpan = endTime - time;

                self.Text_RefreshTime.SetText("{0}:{1}:{2}后刷新", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                await self.Root().GetComponent<TimerComponent>().WaitAsync(1000);
            }
        }

        private static void UpdateStoreList(this UIStoreComponent self)
        {
            while (self.UIStoreItemList.Count < self.StoreItemList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIStoreItem, self.Content_UIStoreITem);
                UIStoreItem newItem = self.AddChild<UIStoreItem, GameObject>(go);
                self.UIStoreItemList.Add(newItem);
            }

            int index = 0;
            foreach (var item in self.StoreItemList)
            {
                self.UIStoreItemList[index].UpdateInfo(item.Key, item.Value, (id) => { self.OnBuy(id).Coroutine(); }).Coroutine();
                self.UIStoreItemList[index].GameObject.SetActive(true);
                index++;
            }

            for (int i = self.StoreItemList.Count; i < self.UIStoreItemList.Count; i++)
            {
                self.UIStoreItemList[i].GameObject.SetActive(false);
            }
        }

        private static async ETTask OnBuy(this UIStoreComponent self, int storeItemConfigId)
        {
            int error = await ClientStoreHelper.StoreBuy(self.Root(), storeItemConfigId);
            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.Root().GetComponent<FloatingTextComponent>().ShowTipText("购买成功！");
            self.StoreItemList[storeItemConfigId]--;

            self.UpdateStoreList();
        }

        public static async ETTask StoreRefreshHandler(this UIStoreComponent self)
        {
            M2C_RefreshStore response = await ClientStoreHelper.RefreshStore(self.Root());
            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.StoreRefreshNum = response.RefreshNum;
            self.StoreItemList = response.StoreItemList;

            self.UpdateStoreList();
        }

        private static async ETTask OnButton_StoreRefresh(this UIStoreComponent self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIStoreRefTip);
            UIStoreRefTipComponent uiStoreRefTipComponent = ui.GetComponent<UIStoreRefTipComponent>();
            uiStoreRefTipComponent.Init(self.StoreRefreshNum);
        }

        private static void OnClose(this UIStoreComponent self)
        {
            self.Dotween_Close.DOLocalMoveY(-100, 0.2f);
            self.Dotween_Scroll.DOLocalMoveY(1750, 0.2f).OnComplete(() => self.Root().GetComponent<UIComponent>().Remove(UIType.UIStore));
        }
    }
}