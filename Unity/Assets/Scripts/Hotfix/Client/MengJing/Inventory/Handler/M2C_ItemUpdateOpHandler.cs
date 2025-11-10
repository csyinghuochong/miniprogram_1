namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_ItemUpdateOpHandler : MessageHandler<Scene, M2C_ItemUpdateOp>
    {
        protected override async ETTask Run(Scene root, M2C_ItemUpdateOp message)
        {
            InventoryComponentC inventoryComponentC = root.GetComponent<InventoryComponentC>();

            foreach (ItemInfo itemInfo in message.ItemInfoRemoveList)
            {
                inventoryComponentC.RemoveItemById(itemInfo.Id);
            }

            foreach (ItemInfo itemInfo in message.ItemInfoUpdateList)
            {
                inventoryComponentC.UpdateItem(itemInfo);
            }

            foreach (ItemInfo itemInfo in message.ItemInfoAddList)
            {
                inventoryComponentC.AddItemFromMessage(itemInfo);
            }

            EventSystem.Instance.Publish(root, new InventoryUpdate());
            await ETTask.CompletedTask;
        }
    }
}