namespace ET.Server
{
    public static class ItemHelper
    {
        public static void SyncItemInfo(Unit unit, Item item, ItemOpType itemOpType)
        {
            M2C_ItemUpdateOp message = M2C_ItemUpdateOp.Create();
            if (itemOpType == ItemOpType.Remove)
            {
                message.ItemInfoRemoveList.Add(item.ToMessage());
            }

            if (itemOpType == ItemOpType.Update)
            {
                message.ItemInfoUpdateList.Add(item.ToMessage());
            }

            if (itemOpType == ItemOpType.Add)
            {
                message.ItemInfoAddList.Add(item.ToMessage());
            }

            MapMessageHelper.SendToClient(unit, message);
        }
    }
}