using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIBattlePassItem))]
    [FriendOf(typeof(UIBattlePassItem))]
    public static partial class UIBattlePassItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIBattlePassItem self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.GameObject = gameObject;

            self.Text_LV = rc.Get<GameObject>("Text_LV").GetComponent<TMP_Text>();
            self.Transform_Reward1 = rc.Get<GameObject>("Transform_Reward1").transform;
            self.Transform_Reward2 = rc.Get<GameObject>("Transform_Reward2").transform;
            self.Transform_Reward3 = rc.Get<GameObject>("Transform_Reward3").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.GameObject_NotCompleted = rc.Get<GameObject>("GameObject_NotCompleted");
            self.Button_OnClick = rc.Get<GameObject>("Button_OnClick").GetComponent<Button>();
        }
    }
}