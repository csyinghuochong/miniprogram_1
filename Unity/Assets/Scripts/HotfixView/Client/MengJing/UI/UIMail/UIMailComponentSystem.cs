using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [Event(SceneType.Demo)]
    public class MailUpdate_UIMailRefresh : AEvent<Scene, MailUpdate>
    {
        protected override async ETTask Run(Scene scene, MailUpdate args)
        {
            UI ui = scene.GetComponent<UIComponent>().Get(UIType.UIMail);
            if (ui == null)
            {
                return;
            }

            UIMailComponent uiMailComponent = ui.GetComponent<UIMailComponent>();
            uiMailComponent.UpdateMailList();

            await ETTask.CompletedTask;
        }
    }

    [EntitySystemOf(typeof(UIMailComponent))]
    [FriendOf(typeof(UIMailComponent))]
    [FriendOf(typeof(MailComponentC))]
    [FriendOf(typeof(Mail))]
    public static partial class UIMailComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIMailComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_DeleteAll = rc.Get<GameObject>("Button_DeleteAll").GetComponent<Button>();
            self.Button_GetAll = rc.Get<GameObject>("Button_GetAll").GetComponent<Button>();
            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIMailItem = rc.Get<GameObject>("Content_UIMailItem").transform;
            self.UIMailItem = rc.Get<GameObject>("UIMailItem");
            self.UIMailItem.SetActive(false);

            self.Button_DeleteAll.onClick.AddListener(() => { self.OnDeleteAll().Coroutine(); });
            self.Button_GetAll.onClick.AddListener(() => { self.OnGetAll().Coroutine(); });
            self.Button_Close.onClick.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIMail); });

            self.UpdateMailList();
        }

        [EntitySystem]
        private static void Destroy(this UIMailComponent self)
        {
            self.UIMailItemList.Clear();
            self.UIMailItem = null;
        }

        public static void UpdateMailList(this UIMailComponent self)
        {
            MailComponentC mailComponent = self.Root().GetComponent<MailComponentC>();

            List<EntityRef<Mail>> mailList = mailComponent.MailList;

            while (self.UIMailItemList.Count < mailList.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIMailItem, self.Content_UIMailItem);
                UIMailItem newItem = self.AddChild<UIMailItem, GameObject>(go);
                self.UIMailItemList.Add(newItem);
            }

            for (int i = 0; i < mailList.Count; i++)
            {
                self.UIMailItemList[i].UpdateInfo(mailList[i]).Coroutine();
                self.UIMailItemList[i].GameObject.SetActive(true);
            }

            for (int i = mailList.Count; i < self.UIMailItemList.Count; i++)
            {
                self.UIMailItemList[i].GameObject.SetActive(false);
            }
        }

        private static async ETTask OnDeleteAll(this UIMailComponent self)
        {
            MailComponentC mailComponent = self.Root().GetComponent<MailComponentC>();
            List<long> mailIds = new List<long>();
            foreach (Mail mail in mailComponent.MailList)
            {
                if (mail.MailReadState == (int)MailReadState.Unread)
                {
                    continue;
                }

                // 已读未领取道具不用删除
                if (mail.MailRewardState == (int)MailRewardState.NotReceived)
                {
                    continue;
                }

                mailIds.Add(mail.Id);
            }

            int error = await ClientMailHelper.OpeMail(self.Root(), MailOpType.Delete, mailIds);
            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.Root().GetComponent<FloatingTextComponent>().ShowTipText("删除已读！");
        }

        private static async ETTask OnGetAll(this UIMailComponent self)
        {
            MailComponentC mailComponent = self.Root().GetComponent<MailComponentC>();
            List<long> mailIds = new List<long>();
            foreach (Mail mail in mailComponent.MailList)
            {
                if (mail.MailRewardState != (int)MailRewardState.NotReceived)
                {
                    continue;
                }

                mailIds.Add(mail.Id);
            }

            int error = await ClientMailHelper.OpeMail(self.Root(), MailOpType.Received, mailIds);
            if (error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.Root().GetComponent<FloatingTextComponent>().ShowTipText("领取全部！");
        }
    }
}