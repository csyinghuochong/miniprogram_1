namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_UseItemHandler : MessageLocationHandler<Unit, C2M_UseItem, M2C_UseItem>
    {
        protected override async ETTask Run(Unit unit, C2M_UseItem request, M2C_UseItem response)
        {
            InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();

            Item item = inventoryComponent.GetItem(request.ItemId);

            if (item == null)
            {
                response.Error = ErrorCode.ERR_NotExistItem;
                return;
            }

            if (item.ContainerType != (int)InventoryContainerType.Bag)
            {
                response.Error = ErrorCode.ERR_InventoryContainerError;
                return;
            }

            if (request.Num < 1 || request.Num > item.Num)
            {
                response.Error = ErrorCode.ERR_ItemUseNumError;
                return;
            }

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);
            if (itemConfig.ItemType == (int)ItemType.Consume)
            {
                // 英雄经验
                if (itemConfig.ItemSubType == (int)ItemConsumeType.HeroExp)
                {
                    Hero hero = unit.GetComponent<HeroComponentS>().GetHero(request.HeroId);

                    if (hero == null)
                    {
                        response.Error = ErrorCode.ERR_NotExistHero;
                        return;
                    }

                    inventoryComponent.RemoveItem(request.ItemId, request.Num);

                    HeroHelper.AddHeroExp(hero, request.Num * int.Parse(itemConfig.ItemUsePar));
                    HeroHelper.UpdateHeroNumeric(unit, hero);
                    HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Update);
                }
                
                // 英雄新级
            }

            await ETTask.CompletedTask;
        }
    }
}