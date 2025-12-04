using System;
using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMailItem))]
    [FriendOf(typeof(UIMailItem))]
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailRewardComponent))]
    public static partial class UIMailItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIMailItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.Text_State = rc.Get<GameObject>("Text_State").GetComponent<TMP_Text>();
            self.Text_Title = rc.Get<GameObject>("Text_Title").GetComponent<TMP_Text>();
            self.Text_Time = rc.Get<GameObject>("Text_Time").GetComponent<TMP_Text>();
            self.Text_DeleteTime = rc.Get<GameObject>("Text_DeleteTime").GetComponent<TMP_Text>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Button_OnClick = rc.Get<GameObject>("Button_OnClick").GetComponent<Button>();

            self.Button_OnClick.onClick.AddListener(() => { self.OnClickHandler().Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIMailItem self)
        {
            self.UIRewardItemList.Clear();
            self.UICommonItem = null;
        }

        public static async ETTask UpdateInfo(this UIMailItem self, Mail mail)
        {
            self.MailId = mail.Id;

            self.Text_Title.SetText(mail.Title);

            self.Text_State.SetText(mail.MailReadState == (int)MailReadState.Unread ? "未读" : "已读");

            DateTime time = TimeInfo.Instance.ToDateTime(mail.Time);
            self.Text_Time.SetTextFormat("{0}-{1}-{2}", time.Year, time.Month, time.Day);

            DateTime endTime = TimeInfo.Instance.ToDateTime(mail.EndTime);
            TimeSpan timeSpan = endTime - time;

            if (timeSpan.TotalDays >= 1)
            {
                self.Text_DeleteTime.SetTextFormat("{0}天后删除", Math.Floor(timeSpan.TotalDays));
            }
            else
            {
                self.Text_DeleteTime.SetTextFormat("{0}小时后删除", Math.Round(timeSpan.TotalHours));
            }

            self.UpdateRewardItemList();

            await ETTask.CompletedTask;
        }

        private static void UpdateRewardItemList(this UIMailItem self)
        {
            Mail mail = self.Root().GetComponent<MailComponentC>().GetMail(self.MailId);

            List<EntityRef<Item>> itemList = mail.GetComponent<MailRewardComponent>().ItemList;

            while (self.UIRewardItemList.Count < itemList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Content_UICommonItem);
                UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
                self.UIRewardItemList.Add(newItem);
            }

            for (int i = 0; i < itemList.Count; i++)
            {
                self.UIRewardItemList[i].UpdateInfo(itemList[i]).Coroutine();
                self.UIRewardItemList[i].GameObject.SetActive(true);

                self.UIRewardItemList[i].Image_Selected.gameObject.SetActive(mail.MailRewardState == (int)MailRewardState.Received);
            }

            for (int i = itemList.Count; i < self.UIRewardItemList.Count; i++)
            {
                self.UIRewardItemList[i].GameObject.SetActive(false);
            }
        }

        private static async ETTask OnClickHandler(this UIMailItem self)
        {
            UI ui = await self.Root().GetComponent<UIComponent>().Create(UIType.UIMailContent);
            UIMailContentComponent uIMailContentComponent = ui.GetComponent<UIMailContentComponent>();
            uIMailContentComponent.Init(self.MailId);
        }
    }
}