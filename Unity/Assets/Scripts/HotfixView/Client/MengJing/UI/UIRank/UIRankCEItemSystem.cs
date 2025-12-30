using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRankCEItem))]
    [FriendOf(typeof(UIRankCEItem))]
    public static partial class UIRankCEItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIRankCEItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Image_PlayerHead = rc.Get<GameObject>("Image_PlayerHead").GetComponent<Image>();
            self.Button_OnPlayerHead = rc.Get<GameObject>("Button_OnPlayerHead").GetComponent<Button>();
            self.Text_PlayerName = rc.Get<GameObject>("Text_PlayerName").GetComponent<TMP_Text>();
            self.Text_PlayerCE = rc.Get<GameObject>("Text_PlayerCE").GetComponent<TMP_Text>();
            self.Text_Sort = rc.Get<GameObject>("Text_Sort").GetComponent<TMP_Text>();
            
        }
        
    }
}