namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class Mail2M_ReceiveRewardHandler : MessageHandler<Unit, Mail2M_ReceiveReward, M2Mail_ReceiveReward>
    {
        protected override async ETTask Run(Unit unit, Mail2M_ReceiveReward request, M2Mail_ReceiveReward response)
        {
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();
            response.Error = inventoryComponent.AddItemData(request.ItemInfoList);

            await ETTask.CompletedTask;
        }
    }
}