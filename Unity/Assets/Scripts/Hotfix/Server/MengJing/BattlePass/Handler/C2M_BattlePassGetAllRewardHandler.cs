using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(BattlePassComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_BattlePassGetAllRewardHandler : MessageLocationHandler<Unit, C2M_BattlePassGetAllReward, M2C_BattlePassGetAllReward>
    {
        protected override async ETTask Run(Unit unit, C2M_BattlePassGetAllReward request, M2C_BattlePassGetAllReward response)
        {
            BattlePassComponent battlePassComponent = unit.GetComponent<BattlePassComponent>();
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            int lv = userInfoComponent.GetLv();
            int recharge = numericComponent.GetAsInt(NumericType.RechargeNumber);

            List<RewardItem> rewardItemList = new List<RewardItem>();
            foreach (BattlePass battlePass in battlePassComponent.BattlePassList)
            {
                BattlePassConfig battlePassConfig = BattlePassConfigCategory.Instance.Get(battlePass.ConfigId);

                if (lv < battlePassConfig.RequiredLv)
                {
                    continue;
                }

                bool notice = false;
                if (!battlePass.RewardReceived_1)
                {
                    notice = true;
                    battlePass.RewardReceived_1 = true;
                    rewardItemList.Add(battlePassConfig.RewardItem1);
                }

                if (recharge >= ConfigData.BattlePassRecharge_2 && !battlePass.RewardReceived_2)
                {
                    notice = true;
                    battlePass.RewardReceived_2 = true;
                    rewardItemList.Add(battlePassConfig.RewardItem2);
                }

                if (recharge >= ConfigData.BattlePassRecharge_3 && !battlePass.RewardReceived_3)
                {
                    notice = true;
                    battlePass.RewardReceived_3 = true;
                    rewardItemList.Add(battlePassConfig.RewardItem3);
                }

                if (notice)
                {
                    response.BattlePassInfoList.Add(battlePass.ToMessage());
                }
            }

            inventoryComponent.AddItemData(rewardItemList);

            await ETTask.CompletedTask;
        }
    }
}