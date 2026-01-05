using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_LotteryDrawRequestHandler : MessageLocationHandler<Unit, C2M_LotteryDrawRequest, M2C_LotteryDrawRequest>
    {
        protected override async ETTask Run(Unit unit, C2M_LotteryDrawRequest request, M2C_LotteryDrawRequest response)
        {
            InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();
            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();

            int drawNum = 0;
            if (request.OpType == 0)
            {
                // 单抽
                drawNum = 1;

                long nowTime = TimeHelper.ServerNow();
                if (nowTime > numericComponent.GetAsInt(NumericType.LotteryDrawFreeTime))
                {
                    // 免费抽
                    numericComponent.ApplyValue(NumericType.LotteryDrawFreeTime, nowTime + ConfigData.LotteryDrawFreeTime);
                }
                else
                {
                    if (!inventoryComponent.HaveItemData(ConfigData.LotteryDrawCost_One))
                    {
                        response.Error = ErrorCode.ERR_NotEnoughItems;
                        return;
                    }

                    inventoryComponent.RemoveItemData(ConfigData.LotteryDrawCost_One);
                }
            }
            else if (request.OpType == 1)
            {
                // 十连
                drawNum = 10;

                if (!inventoryComponent.HaveItemData(ConfigData.LotteryDrawCost_Ten))
                {
                    response.Error = ErrorCode.ERR_NotEnoughItems;
                    return;
                }

                inventoryComponent.RemoveItemData(ConfigData.LotteryDrawCost_Ten);
            }
            else
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            List<RewardItem> rewardItemList = new();

            int currentLotteryDrawNum = numericComponent.GetAsInt(NumericType.LotteryDrawNum);
            for (int i = 0; i < drawNum; i++)
            {
                int dropId = 0;
                if (currentLotteryDrawNum + i + 1 == ConfigData.LotteryDrawBaoDi)
                {
                    // 保底
                    dropId = ConfigData.LotteryDrawBaoDiDropId;
                }
                else
                {
                    dropId = ConfigData.LotteryDrawDropId;
                }

                rewardItemList.Add(DropHelper.DropItem(dropId));
            }

            if (currentLotteryDrawNum + drawNum >= ConfigData.LotteryDrawBaoDi)
            {
                numericComponent.ApplyValue(NumericType.LotteryDrawNum, currentLotteryDrawNum + drawNum - ConfigData.LotteryDrawBaoDi);
            }
            else
            {
                numericComponent.ApplyValue(NumericType.LotteryDrawNum, currentLotteryDrawNum + drawNum);
            }

            inventoryComponent.AddItemData(rewardItemList);

            foreach (RewardItem rewardItem in rewardItemList)
            {
                ItemInfo itemInfo = ItemInfo.Create();
                itemInfo.ConfigId = rewardItem.ItemId;
                itemInfo.Num = rewardItem.ItemNum;

                response.ItemInfoList.Add(itemInfo);
            }

            await ETTask.CompletedTask;
        }
    }
}