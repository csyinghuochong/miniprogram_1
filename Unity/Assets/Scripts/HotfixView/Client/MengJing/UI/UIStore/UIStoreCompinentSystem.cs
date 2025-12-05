using System;
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

            self.Text_Type_Gold = rc.Get<GameObject>("Text_Type_Gold").GetComponent<TMP_Text>();
            self.Text_Type_Diamond = rc.Get<GameObject>("Text_Type_Diamond").GetComponent<TMP_Text>();
            self.Button_AddGold = rc.Get<GameObject>("Button_AddGold").GetComponent<Button>();
            self.Button_AddDiamond = rc.Get<GameObject>("Button_AddDiamond").GetComponent<Button>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Text_RefreshTime = rc.Get<GameObject>("Text_RefreshTime").GetComponent<TMP_Text>();
            self.Button_RefreshTime = rc.Get<GameObject>("Button_Refresh").GetComponent<Button>();
            self.Content_UIStoreITem = rc.Get<GameObject>("Content_UIStoreItem").transform;
            self.UIStoreItem = rc.Get<GameObject>("UIStoreItem");
            self.UIStoreItem.SetActive(false);

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIStore); });

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
                self.UIStoreItemList[index].UpdateInfo(item.Key, item.Value);
                self.UIStoreItemList[index].GameObject.SetActive(true);
                index++;
            }

            for (int i = self.StoreItemList.Count; i < self.UIStoreItemList.Count; i++)
            {
                self.UIStoreItemList[i].GameObject.SetActive(false);
            }
        }
    }
}