namespace ET.Client
{
    [MessageHandler(SceneType.Demo)]
    public class M2C_ItemUpdateOpHandler : MessageHandler<Scene, M2C_ItemUpdateOp>
    {
        protected override async ETTask Run(Scene root, M2C_ItemUpdateOp message)
        {
            InventoryComponentC inventoryComponentC = root.GetComponent<InventoryComponentC>();

            if (message.ItemOpType == (int)ItemOpType.Add)
            {
                inventoryComponentC.AddItemFromMessage(message.ItemInfo);
            }
            else if (message.ItemOpType == (int)ItemOpType.Remove)
            {
                inventoryComponentC.RemoveItemById(message.ItemInfo.Id);
            }
            else if (message.ItemOpType == (int)ItemOpType.Update)
            {
                inventoryComponentC.UpdateItem(message.ItemInfo);
            }

            await ETTask.CompletedTask;
        }
    }
}