using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIPlayerInfoComponent))]
    [FriendOf(typeof(UIPlayerInfoComponent))]
    public static partial class UIPlayerInfoSystem
    {
        [EntitySystem]
        private static void Awake(this UIPlayerInfoComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Image_PlayerHead = rc.Get<GameObject>("Image_PlayerHead").GetComponent<Image>();
            self.Button_OnPlayerHead = rc.Get<GameObject>("Button_OnPlayerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerCE = rc.Get<GameObject>("Text_PlayerCE").GetComponent<TMP_Text>();
            self.Text_PlayerLianMeng = rc.Get<GameObject>("Text_PlayerLianMeng").GetComponent<TMP_Text>();
            self.Button_AddFriend = rc.Get<GameObject>("Button_AddFriend").GetComponent<Button>();
            self.Button_DeleteFriend = rc.Get<GameObject>("Button_DeleteFriend").GetComponent<Button>();
            self.Button_Report = rc.Get<GameObject>("Button_Report").GetComponent<Button>();
            self.Button_Black = rc.Get<GameObject>("Button_Black").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIPlayerInfo); });
            self.Button_AddFriend.AddListener(() => { self.OnButton_AddFriend().Coroutine(); });
            self.Button_DeleteFriend.AddListener(() => { self.OnButton_DeleteFriend().Coroutine(); });
            self.Button_Report.AddListener(() => { self.OnButton_Report().Coroutine(); });
            self.Button_Black.AddListener(() => { self.OnButton_Black().Coroutine(); });
        }

        [EntitySystem]
        private static void Destroy(this UIPlayerInfoComponent self)
        {
            self.WatchPlayerInfo = null;
        }

        public static void UpdateInfo(this UIPlayerInfoComponent self, WatchPlayerInfo watchPlayerInfo)
        {
            self.WatchPlayerInfo = watchPlayerInfo;
            self.Text_PlayerName.SetText(self.WatchPlayerInfo.PlayerName);
            self.Text_PlayerCE.SetTextFormat("战力:{0}", self.WatchPlayerInfo.CombatPower);

            // self.WatchPlayerInfo.HeroFormation
            // self.WatchPlayerInfo.HeroInfoList

            bool isMy = self.WatchPlayerInfo.UnitId == self.Root().GetComponent<PlayerInfoComponent>().CurrentRoleId;
            if (isMy)
            {
                self.Button_AddFriend.gameObject.SetActive(false);
                self.Button_DeleteFriend.gameObject.SetActive(false);
                self.Button_Report.gameObject.SetActive(false);
                self.Button_Black.gameObject.SetActive(false);
            }
            else
            {
                FriendComponentC friendComponent = self.Root().GetComponent<FriendComponentC>();
                bool isFriend = friendComponent.IsFriend(self.WatchPlayerInfo.UnitId);
                self.Button_AddFriend.gameObject.SetActive(!isFriend);
                self.Button_DeleteFriend.gameObject.SetActive(isFriend);
            }
        }

        private static async ETTask OnButton_AddFriend(this UIPlayerInfoComponent self)
        {
            int error = await ClientFriendHelper.FriendRequest(self.Root(), self.WatchPlayerInfo.UnitId);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("申请成功");
            }
        }

        private static async ETTask OnButton_DeleteFriend(this UIPlayerInfoComponent self)
        {
            int error = await ClientFriendHelper.DeleteFriend(self.Root(), self.WatchPlayerInfo.UnitId);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("删除成功");
            }
        }

        private static async ETTask OnButton_Report(this UIPlayerInfoComponent self)
        {
            await ETTask.CompletedTask;
        }

        private static async ETTask OnButton_Black(this UIPlayerInfoComponent self)
        {
            await ETTask.CompletedTask;
        }
    }
}