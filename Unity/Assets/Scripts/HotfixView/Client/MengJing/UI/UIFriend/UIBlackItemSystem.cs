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
            
        }
        
    }
}