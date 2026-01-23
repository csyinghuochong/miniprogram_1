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

            self.Text_RequiredLv = rc.Get<GameObject>("Text_RequiredLv").GetComponent<TMP_Text>();
            self.Transform_Reward1 = rc.Get<GameObject>("Transform_Reward1").transform;
            self.Transform_Reward2 = rc.Get<GameObject>("Transform_Reward2").transform;
            self.Transform_Reward3 = rc.Get<GameObject>("Transform_Reward3").transform;
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.GameObject_NotCompleted = rc.Get<GameObject>("GameObject_NotCompleted");
            self.GameObject_NotCompleted.SetActive(false);
            self.Button_OnClick = rc.Get<GameObject>("Button_OnClick").GetComponent<Button>();
        }

        public static void UpdateInfo(this UIBattlePassItem self, int rewardId)
        {
            self.RewardId = rewardId;

            BattlePassConfig battlePassConfig = BattlePassConfigCategory.Instance.Get(self.RewardId);

            self.Text_RequiredLv.SetText(battlePassConfig.RequiredLv.ToString());


            RewardItem rewardItem1 = battlePassConfig.RewardItem1;
            RewardItem rewardItem2 = battlePassConfig.RewardItem2;
            RewardItem rewardItem3 = battlePassConfig.RewardItem3;

            GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Transform_Reward1);
            UICommonItem newItem = self.AddChild<UICommonItem, GameObject>(go);
            self.uiCommonItem = newItem;
            self.uiCommonItem.UpdateInfo(rewardItem1.ItemId, rewardItem1.ItemNum).Coroutine();
            self.uiCommonItem.GameObject.SetActive(true);
            
            go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Transform_Reward2);
            newItem = self.AddChild<UICommonItem, GameObject>(go);
            self.uiCommonItem = newItem;
            self.uiCommonItem.UpdateInfo(rewardItem2.ItemId, rewardItem2.ItemNum).Coroutine();
            self.uiCommonItem.GameObject.SetActive(true);
            
            go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Transform_Reward3);
            newItem = self.AddChild<UICommonItem, GameObject>(go);
            self.uiCommonItem = newItem;
            self.uiCommonItem.UpdateInfo(rewardItem3.ItemId, rewardItem3.ItemNum).Coroutine();
            self.uiCommonItem.GameObject.SetActive(true);
        }
    }
}