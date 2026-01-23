using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_RechargeHandler : MessageLocationHandler<Unit, C2M_Recharge, M2C_Recharge>
    {
        protected override async ETTask Run(Unit unit, C2M_Recharge request, M2C_Recharge response)
        {
            if (!RechargeConfigCategory.Instance.DataMap.ContainsKey(request.ConfigId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            RechargeConfig rechargeConfig = RechargeConfigCategory.Instance.Get(request.ConfigId);

            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();
            inventoryComponent.AddItemData(new List<RewardItem>() { new() { ItemId = ConfigData.Item_Diamond, ItemNum = rechargeConfig.Diamond } });

            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            numericComponent.ApplyChange(NumericType.RechargeNumber, rechargeConfig.Price);
            numericComponent.ApplyChange(NumericType.RechargePoint, rechargeConfig.Point);

            await ETTask.CompletedTask;
        }
    }
}