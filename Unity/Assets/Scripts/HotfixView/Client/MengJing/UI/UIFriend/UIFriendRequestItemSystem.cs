using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFriendRequestItem))]
    [FriendOf(typeof(UIFriendRequestItem))]
    public static partial class UIFriendRequestSystem
    {
        [EntitySystem]
        private static void Awake(this UIFriendRequestItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_PlayerHead = rc.Get<GameObject>("Image_PlayerHead").GetComponent<Image>();
            self.Button_OnPlayerHead = rc.Get<GameObject>("Button_OnPlayerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerLv = rc.Get<GameObject>("Text_PlayerLv").GetComponent<TMP_Text>();
            self.Text_PlayerCE = rc.Get<GameObject>("Text_PlayerCE").GetComponent<TMP_Text>();
            self.Text_PlayerStatus = rc.Get<GameObject>("Text_PlayerStatus").GetComponent<TMP_Text>();
            self.Button_Accept = rc.Get<GameObject>("Button_Accept").GetComponent<Button>();
            self.Button_Refuse = rc.Get<GameObject>("Button_Refuse").GetComponent<Button>();
        }

        public static void UpdateInfo(this UIFriendRequestItem self, FriendData friendData)
        {
            self.FriendData = friendData;

            self.Text_PlayerName.SetText(friendData.PlayerName);
            self.Text_PlayerLv.SetTextFormat("等级:{0}", friendData.Lv);
            self.Text_PlayerStatus.SetText(friendData.OnLine == 1 ? "在线" : "离线");
        }
    }
}