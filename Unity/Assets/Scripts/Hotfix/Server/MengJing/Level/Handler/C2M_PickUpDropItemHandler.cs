using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_PickUpDropItemHandler : MessageLocationHandler<Unit, C2M_PickUpDropItem, M2C_PickUpDropItem>
    {
        protected override async ETTask Run(Unit unit, C2M_PickUpDropItem request, M2C_PickUpDropItem response)
        {
            using (await unit.Root().GetComponent<CoroutineLockComponent>().Wait(CoroutineLockType.PickUpDropItem, unit.Id))
            {
                if (unit.IsDisposed)
                {
                    return;
                }

                List<RewardItem> rewardItems = new List<RewardItem>();
                UnitComponent unitComponent = unit.GetParent<UnitComponent>();
                foreach (long unitId in request.UnitIdList)
                {
                    Unit dropItem = unitComponent.Get(unitId);
                    if (dropItem == null)
                    {
                        continue;
                    }

                    if (dropItem.Type != UnitType.DropItem)
                    {
                        continue;
                    }

                    NumericComponentS numericComponent = dropItem.GetComponent<NumericComponentS>();
                    rewardItems.Add(new RewardItem()
                    {
                        ItemId = numericComponent.GetAsInt(NumericType.DropItemId), ItemNum = numericComponent.GetAsInt(NumericType.DropItemNum)
                    });

                    unitComponent.Remove(unitId);
                }

                if (rewardItems.Count > 0)
                {
                    unit.GetComponent<InventoryComponentS>().AddItemData(rewardItems);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}