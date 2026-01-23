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

            self.Button_OnClick.AddListener(() => { self.OnButton_OnClick().Coroutine(); });

            GameObject go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Transform_Reward1);
            go.SetActive(true);
            self.UICommonItem_1 = self.AddChild<UICommonItem, GameObject>(go);
            go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Transform_Reward2);
            go.SetActive(true);
            self.UICommonItem_2 = self.AddChild<UICommonItem, GameObject>(go);
            go = UnityEngine.Object.Instantiate(self.UICommonItem, self.Transform_Reward3);
            go.SetActive(true);
            self.UICommonItem_3 = self.AddChild<UICommonItem, GameObject>(go);
        }

        public static void UpdateInfo(this UIBattlePassItem self, int rewardId)
        {
            self.RewardId = rewardId;

            BattlePassConfig battlePassConfig = BattlePassConfigCategory.Instance.Get(self.RewardId);
            BattlePassComponentC battlePassComponent = self.Root().GetComponent<BattlePassComponentC>();

            BattlePass battlePass = battlePassComponent.GetBattlePass(rewardId);

            self.Text_RequiredLv.SetText(battlePassConfig.RequiredLv.ToString());

            RewardItem rewardItem1 = battlePassConfig.RewardItem1;
            RewardItem rewardItem2 = battlePassConfig.RewardItem2;
            RewardItem rewardItem3 = battlePassConfig.RewardItem3;

            self.UICommonItem_1.UpdateInfo(rewardItem1.ItemId, rewardItem1.ItemNum).Coroutine();
            self.UICommonItem_1.Image_Selected.gameObject.SetActive(battlePass != null && battlePass.RewardReceived_1);

            self.UICommonItem_2.UpdateInfo(rewardItem2.ItemId, rewardItem2.ItemNum).Coroutine();
            self.UICommonItem_2.Image_Selected.gameObject.SetActive(battlePass != null && battlePass.RewardReceived_2);

            self.UICommonItem_3.UpdateInfo(rewardItem3.ItemId, rewardItem3.ItemNum).Coroutine();
            self.UICommonItem_3.Image_Selected.gameObject.SetActive(battlePass != null && battlePass.RewardReceived_3);

            UserInfoComponentC userInfoComponent = self.Root().GetComponent<UserInfoComponentC>();
            NumericComponentC numericComponent = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<NumericComponentC>();

            int lv = userInfoComponent.Lv;
            int recharge = numericComponent.GetAsInt(NumericType.RechargeNumber);
            if (lv >= battlePassConfig.RequiredLv && (!battlePass.RewardReceived_1 ||
                    recharge >= ConfigData.BattlePassRecharge_2 && !battlePass.RewardReceived_2 ||
                    recharge >= ConfigData.BattlePassRecharge_3 && !battlePass.RewardReceived_3))
            {
                // 有道具可领取
            }
            else
            {
                // 没有道具可领取
            }
        }

        private static async ETTask OnButton_OnClick(this UIBattlePassItem self)
        {
            int error = await ClientBattlePassHelper.BattlePassGetReward(self.Root(), self.RewardId);
            if (error == ErrorCode.ERR_Success)
            {
                self.UpdateInfo(self.RewardId);
            }
        }
    }
}