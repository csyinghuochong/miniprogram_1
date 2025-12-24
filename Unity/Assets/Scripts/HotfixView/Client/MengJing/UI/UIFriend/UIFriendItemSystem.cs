using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIFriendItem))]
    [FriendOf(typeof(UIFriendItem))]
    public static partial class UIFriendItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIFriendItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_PlayerHead = rc.Get<GameObject>("Image_PlayerHead").GetComponent<Image>();
            self.Button_OnPlayerHead = rc.Get<GameObject>("Button_OnPlayerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerLv = rc.Get<GameObject>("Text_PlayerLv").GetComponent<TMP_Text>();
            self.Text_PlayerCE = rc.Get<GameObject>("Text_PlayerCE").GetComponent<TMP_Text>();
            self.Text_PlayerStatus = rc.Get<GameObject>("Text_PlayerStatus").GetComponent<TMP_Text>();
            self.Button_Chat = rc.Get<GameObject>("Button_Chat").GetComponent<Button>();
            self.Text_Sort = rc.Get<GameObject>("Text_Sort").GetComponent<TMP_Text>();
        }

        public static void UpdateInfo(this UIFriendItem self, FriendData friendData)
        {
            self.FriendData = friendData;

            self.Text_PlayerName.SetText(friendData.PlayerName);
            self.Text_PlayerStatus.SetText(friendData.OnLine == 1 ? "在线" : "离线");
            self.Text_PlayerLv.SetTextFormat("等级:{0}", friendData.Lv);
            self.Text_PlayerCE.SetTextFormat("战力:{0}", friendData.CombatPower);
        }
    }
}