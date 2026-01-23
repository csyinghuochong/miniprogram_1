using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRechargePointsRewardComponent))]
    [FriendOf(typeof(UIRechargePointsRewardComponent))]
    public static partial class UIRechargePointsRewardComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIRechargePointsRewardComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIRechargePointsRewardItem = rc.Get<GameObject>("Content_UIRechargePointsRewardItem").transform;
            self.UIRechargePointsRewardItem = rc.Get<GameObject>("UIRechargePointsRewardItem");
            self.UIRechargePointsRewardItem.SetActive(false);
            self.Text_VipLv = rc.Get<GameObject>("Text_VipLv").GetComponent<TMP_Text>();
            self.Image_PointsProgress = rc.Get<GameObject>("Image_PointsProgress").GetComponent<Image>();
            self.Text_Points = rc.Get<GameObject>("Text_Points").GetComponent<TMP_Text>();

            self.Button_Close.AddListener(() => { self.GameObject.SetActive(false); });
            
        }

        [EntitySystem]
        private static void Destroy(this UIRechargePointsRewardComponent self)
        {
            self.UIRechargePointsRewardItemList.Clear();
            self.UIRechargePointsRewardItem = null;
        }
        
        
    }
}