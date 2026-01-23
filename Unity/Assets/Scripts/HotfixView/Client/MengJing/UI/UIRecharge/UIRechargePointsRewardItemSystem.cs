using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRechargePointsRewardItem))]
    [FriendOf(typeof(UIRechargePointsRewardItem))]
    public static partial class UIRechargePointsRewardItemSystem
    {
        [EntitySystem]
        private static void Awake(this UIRechargePointsRewardItem self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Content_UICommonItem = rc.Get<GameObject>("Content_UICommonItem").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.Text_RequiredPoints = rc.Get<GameObject>("Text_RequiredPoints").GetComponent<TMP_Text>();
            self.GameObject_Received = rc.Get<GameObject>("GameObject_Received");
            self.Button_GetReward = rc.Get<GameObject>("Button_GetReward").GetComponent<Button>();
        }

        [EntitySystem]
        private static void Destroy(this UIRechargePointsRewardItem self)
        {
            self.UIRewardItemList.Clear();
            self.UICommonItem = null;
        }
    }
}