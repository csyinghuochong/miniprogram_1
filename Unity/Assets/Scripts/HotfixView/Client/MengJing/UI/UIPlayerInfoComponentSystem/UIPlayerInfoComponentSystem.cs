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
            self.Button_Report = rc.Get<GameObject>("Button_Report").GetComponent<Button>();
            self.Button_Black = rc.Get<GameObject>("Button_Black").GetComponent<Button>();

            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIPlayerInfo); });
        }


    }
}