using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIRechargePointsComponent))]
    [FriendOf(typeof(UIRechargePointsComponent))]
    public static partial class UIRechargePointsComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIRechargePointsComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIRechargePointsItem = rc.Get<GameObject>("Content_UIRechargePointsItem").transform;
            self.UIRechargePointsItem = rc.Get<GameObject>("UIRechargePointsItem");
            self.UIRechargePointsItem.SetActive(false);
            self.Text_VipLv = rc.Get<GameObject>("Text_VipLv").GetComponent<TMP_Text>();
            self.Image_PointsProgress = rc.Get<GameObject>("Image_PointsProgress").GetComponent<Image>();
            self.Text_Points = rc.Get<GameObject>("Text_Points").GetComponent<TMP_Text>();

            self.Button_Close.AddListener(() => { self.GameObject.SetActive(false); });
            
        }

        [EntitySystem]
        private static void Destroy(this UIRechargePointsComponent self)
        {
            self.UIRechargePointsItemList.Clear();
            self.UIRechargePointsItem = null;
        }
        
        
    }
}