using System;
using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIMailContentComponent))]
    [FriendOf(typeof(UIMailContentComponent))]
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailRewardComponent))]
    public static partial class UIMailContentComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMailContentComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Title = rc.Get<GameObject>("Text_Title").GetComponent<TMP_Text>();
            self.Text_From = rc.Get<GameObject>("Text_From").GetComponent<TMP_Text>();
            self.Text_Time = rc.Get<GameObject>("Text_Time").GetComponent<TMP_Text>();
            self.Text_Content = rc.Get<GameObject>("Text_Content").GetComponent<TMP_Text>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Button_Get = rc.Get<GameObject>("Button_Get").GetComponent<Button>();
            self.Button_Delete = rc.Get<GameObject>("Button_Delete").GetComponent<Button>();
            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");

            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIMailContent); });
            self.Button_Get.onClick.AddListener(() => { self.OnGet().Coroutine(); });
            self.Button_Delete.onClick.AddListener(() => { self.OnDelete().Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIMailContentComponent self)
        {
            self.UIRewardItemList.Clear();
            self.UICommonItem = null;
        }

        public static void Init(this UIMailContentComponent self, long mailId)
        {
            self.MailId = mailId;

            MailComponentC mailComponent = self.Root().GetComponent<MailComponentC>();
            Mail mail = mailComponent.GetMail(mailId);

            self.Text_From.SetTextFormat("来自:{0}", mail.From);

            self.Text_Title.SetText(mail.Title);

            DateTime time = TimeInfo.Instance.ToDateTime(mail.Time);
            self.Text_Time.SetTextFormat("时间:{0}-{1}-{2}", time.Year, time.Month, time.Day);

            self.Text_Content.SetText(mail.Content);

            if (mail.MailReadState == (int)MailReadState.Unread)
            {
                ClientMailHelper.OpeMail(self.Root(), MailOpType.Read, new() { mailId }).Coroutine();
            }
            
            self.UpdateRewardItemList();
        }

        private static void UpdateRewardItemList(this UIMailContentComponent self)
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
        
        private static async ETTask OnDelete(this UIMailContentComponent self)
        {
            int error = await ClientMailHelper.OpeMail(self.Root(), MailOpType.Delete, new() { self.MailId });
            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.Root().GetComponent<FloatingTextComponent>().ShowTipText("删除邮件成功！");

            self.Root().GetComponent<UIComponent>().Remove(UIType.UIMailContent);
        }

        private static async ETTask OnGet(this UIMailContentComponent self)
        {
            int error = await ClientMailHelper.OpeMail(self.Root(), MailOpType.Received, new() { self.MailId });
            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.Init(self.MailId);
            
            self.Root().GetComponent<FloatingTextComponent>().ShowTipText("领取邮件道具成功！");
        }
    }
}