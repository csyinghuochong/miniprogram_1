using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMailItem))]
    [FriendOf(typeof(UIMailItem))]
    [FriendOf(typeof(Mail))]
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

            self.Button_OnClick.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Create(UIType.UIMailContent).Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIMailItem self)
        {
            self.UICommonItem = null;
        }

        public static async ETTask UpdateInfo(this UIMailItem self, Mail mail)
        {
            self.MailId = mail.Id;

            self.Text_State.SetText(mail.MailReadState == (int)MailReadState.Unread ? "未读" : "已读");
            DateTime time = TimeInfo.Instance.ToDateTime(mail.Time);
            self.Text_Time.SetTextFormat("{0}-{1}-{2}", time.Year, time.Month, time.Day);

            DateTime EndTime = TimeInfo.Instance.ToDateTime(mail.EndTime);
            self.Text_DeleteTime.SetTextFormat("{0}天后删除", EndTime - time);
        }
    }
}