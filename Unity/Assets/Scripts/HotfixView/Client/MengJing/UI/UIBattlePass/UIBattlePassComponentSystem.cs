using System.Collections.Generic;
using Cysharp.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ET.Client
{
    [EntitySystemOf(typeof(UIBattlePassComponent))]
    [FriendOf(typeof(UIBattlePassComponent))]
    public static partial class UIBattlePassComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIBattlePassComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Close = rc.Get<GameObject>("Button_Close").GetComponent<Button>();
            self.Content_UIBattlePassItem = rc.Get<GameObject>("Content_UIBattlePassItem").transform;
            self.UIBattlePassItem = rc.Get<GameObject>("UIBattlePassItem");
            self.UIBattlePassItem.gameObject.SetActive(false);
            self.Button_GetAllReward = rc.Get<GameObject>("Button_GetAllReward").GetComponent<Button>();

            self.AddComponent<UICommonHuoBiSetComponent, GameObject>(rc.Get<GameObject>("UICommonHuoBiSet"));
            self.Button_Close.AddListener(() => { self.Root().GetComponent<UIComponent>().Remove(UIType.UIBattlePass); });
            
            self.UpdateInfo();
        }

        [EntitySystem]
        private static void Destroy(this UIBattlePassComponent self)
        {
            self.UIBattlePassItemList.Clear();
            self.UIBattlePassItem = null;
        }

        public static void UpdateInfo(this UIBattlePassComponent self)
        {
            List<RechargePointsRewardConfig> rechargePointsRewardConfigs = RechargePointsRewardConfigCategory.Instance.DataList;

            while (self.UIBattlePassItemList.Count < rechargePointsRewardConfigs.Count)
            {
                GameObject go = UnityEngine.Object.Instantiate(self.UIBattlePassItem, self.Content_UIBattlePassItem);
                UIBattlePassItem newItem = self.AddChild<UIBattlePassItem, GameObject>(go);
                self.UIBattlePassItemList.Add(newItem);
            }

            for (int i = 0; i < rechargePointsRewardConfigs.Count; i++)
            {
                self.UIBattlePassItemList[i].UpdateInfo(rechargePointsRewardConfigs[i].Id);
                self.UIBattlePassItemList[i].GameObject.SetActive(true);
            }

            for (int i = rechargePointsRewardConfigs.Count; i < self.UIBattlePassItemList.Count; i++)
            {
                self.UIBattlePassItemList[i].GameObject.SetActive(false);
            }

        }
    }
}