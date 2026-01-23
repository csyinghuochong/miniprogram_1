using System.Collections.Generic;
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
            
            self.UpdateInfo();
            
        }

        [EntitySystem]
        private static void Destroy(this UIRechargePointsRewardComponent self)
        {
            self.UIRechargePointsRewardItemList.Clear();
            self.UIRechargePointsRewardItem = null;
        }
        
        public static void UpdateInfo(this UIRechargePointsRewardComponent self)
        {
            List<RechargePointsRewardConfig> rechargePointsRewardConfigs = RechargePointsRewardConfigCategory.Instance.DataList;

            while (self.UIRechargePointsRewardItemList.Count < rechargePointsRewardConfigs.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIRechargePointsRewardItem, self.Content_UIRechargePointsRewardItem);
                UIRechargePointsRewardItem newItem = self.AddChild<UIRechargePointsRewardItem, GameObject>(go);
                self.UIRechargePointsRewardItemList.Add(newItem);
            }

            for (int i = 0; i < rechargePointsRewardConfigs.Count; i++)
            {
                self.UIRechargePointsRewardItemList[i].UpdateInfo(rechargePointsRewardConfigs[i].Id);
                self.UIRechargePointsRewardItemList[i].GameObject.SetActive(true);
            }

            for (int i = rechargePointsRewardConfigs.Count; i < self.UIRechargePointsRewardItemList.Count; i++)
            {
                self.UIRechargePointsRewardItemList[i].GameObject.SetActive(false);
            }
            
        }
    }
}