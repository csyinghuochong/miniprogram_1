using System.Collections.Generic;

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
                // 获取金币
                if (itemConfig.ItemSubType == (int)ItemConsumeType.GetGold)
                {
                    inventoryComponent.RemoveItem(request.ItemId, request.Num);

                    List<RewardItem> addItems = new List<RewardItem>();
                    addItems.Add(new RewardItem() { ItemId = 1, ItemNum = int.Parse(itemConfig.ItemUsePar) });
                    inventoryComponent.AddItemData(addItems);
                }
                
                // 随机宝箱
                if (itemConfig.ItemSubType == (int)ItemConsumeType.BaoXian)
                {
                    inventoryComponent.RemoveItem(request.ItemId, request.Num);
                    
                    string[] itemList = itemConfig.ItemUsePar.Split(',');
                    int index = RandomHelper.RandomNumber(0, itemList.Length);
                    int itemId = int.Parse(itemList[index]);
                    
                    List<RewardItem> addItems = new List<RewardItem>();
                    addItems.Add(new RewardItem() { ItemId = itemId, ItemNum = 1 });
                    inventoryComponent.AddItemData(addItems);
                }

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

                    string[] expRange = itemConfig.ItemUsePar.Split(',');
                    for (int i = 0; i < request.Num; i++)
                    {
                        int expValue = RandomHelper.RandomNumber(int.Parse(expRange[0]), int.Parse(expRange[1]));
                        HeroHelper.AddHeroExp(hero, request.Num * expValue);
                    }

                    HeroHelper.UpdateHeroNumeric(unit, hero);
                    HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Update);
                }

                // 英雄星级
                if (itemConfig.ItemSubType == (int)ItemConsumeType.HeroHunshi)
                {
                    Hero hero = unit.GetComponent<HeroComponentS>().GetHero(request.HeroId);

                    if (hero == null)
                    {
                        response.Error = ErrorCode.ERR_NotExistHero;
                        return;
                    }

                    inventoryComponent.RemoveItem(request.ItemId, request.Num);

                    string[] hunShiRange = itemConfig.ItemUsePar.Split(',');
                    for (int i = 0; i < request.Num; i++)
                    {
                        int hunShiValue = RandomHelper.RandomNumber(int.Parse(hunShiRange[0]), int.Parse(hunShiRange[1]));
                        HeroHelper.AddHeroHunShi(hero, request.Num * hunShiValue);
                    }

                    HeroHelper.UpdateHeroNumeric(unit, hero);
                    HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Update);
                }
            }

            await ETTask.CompletedTask;
        }
    }
}