using System.Collections.Generic;
using ET.Server;

namespace ET.Client
{
    [FriendOf(typeof(BattlePassComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_BattlePassGetRewardHandler : MessageLocationHandler<Unit, C2M_BattlePassGetReward, M2C_BattlePassGetReward>
    {
        protected override async ETTask Run(Unit unit, C2M_BattlePassGetReward request, M2C_BattlePassGetReward response)
        {
            BattlePassComponent battlePassComponent = unit.GetComponent<BattlePassComponent>();
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            BattlePass battlePass = battlePassComponent.GetBattlePass(request.ConfigId);
            if (battlePass == null)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            int lv = userInfoComponent.GetLv();
            int recharge = numericComponent.GetAsInt(NumericType.RechargeNumber);

            List<RewardItem> rewardItemList = new List<RewardItem>();

            BattlePassConfig battlePassConfig = BattlePassConfigCategory.Instance.Get(battlePass.ConfigId);
            if (lv < battlePassConfig.RequiredLv)
            {
                response.Error = ErrorCode.ERR_NotEnoughLv;
                return;
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
                response.BattlePassInfo = battlePass.ToMessage();
            }
            
            inventoryComponent.AddItemData(rewardItemList);

            await ETTask.CompletedTask;
        }
    }
}