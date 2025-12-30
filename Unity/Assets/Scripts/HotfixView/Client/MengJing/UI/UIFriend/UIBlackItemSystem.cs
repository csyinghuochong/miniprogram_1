using System;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIBlackItem))]
    [FriendOf(typeof(UIBlackItem))]
    public static partial class UIBlackSystem
    {
        [EntitySystem]
        private static void Awake(this UIBlackItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_PlayerHead = rc.Get<GameObject>("Image_PlayerHead").GetComponent<Image>();
            self.Button_OnPlayerHead = rc.Get<GameObject>("Button_OnPlayerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerLv = rc.Get<GameObject>("Text_PlayerLv").GetComponent<TMP_Text>();
            self.Text_PlayerCE = rc.Get<GameObject>("Text_PlayerCE").GetComponent<TMP_Text>();
            self.Text_PlayerStatus = rc.Get<GameObject>("Text_PlayerStatus").GetComponent<TMP_Text>();
            self.Button_CancelBlack = rc.Get<GameObject>("Button_CancelBlack").GetComponent<Button>();

            self.Button_CancelBlack.AddListener(() => { self.OnButton_CancelBlack().Coroutine(); });
        }

        public static void UpdateInfo(this UIBlackItem self, FriendData friendData)
        {
            self.FriendData = friendData;

            self.Text_PlayerName.SetText(friendData.PlayerName);
            if (friendData.OnLine == 1)
            {
                self.Text_PlayerStatus.SetText("在线");
            }
            else
            {
                DateTime lastTime = TimeInfo.Instance.ToDateTime(friendData.LastLoginTime);
                DateTime nowTime = TimeInfo.Instance.ToDateTime(TimeHelper.ServerNow());
                TimeSpan timeSpan = nowTime - lastTime;
                if (timeSpan.Days > 0)
                {
                    self.Text_PlayerStatus.SetTextFormat("{0}天前登录", timeSpan.Days);
                }
                else if (timeSpan.Hours > 0)
                {
                    self.Text_PlayerStatus.SetTextFormat("{0}小时前登录", timeSpan.Hours);
                }
                else
                {
                    self.Text_PlayerStatus.SetTextFormat("{0}分钟前登录", timeSpan.Minutes);
                }
            }

            self.Text_PlayerLv.SetTextFormat("等级:{0}", friendData.Lv);
            self.Text_PlayerCE.SetTextFormat("战力:{0}", friendData.CombatPower);
        }

        private static async ETTask OnButton_CancelBlack(this UIBlackItem self)
        {
            int error = await ClientFriendHelper.BlackFriend(self.Root(), self.FriendData.UnitId, 1);
            if (error == ErrorCode.ERR_Success)
            {
                self.Root().GetComponent<FloatingTextComponent>().ShowTipText("取消拉黑成功");
            }
        }
    }
}