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

            // 获取金币
            if (itemConfig.ItemSubType == ItemSubType.GetGold)
            {
                inventoryComponent.RemoveItem(request.ItemId, request.Num);

                List<RewardItem> addItems = new List<RewardItem>();
                addItems.Add(new RewardItem() { ItemId = ConfigData.Item_Gold, ItemNum = itemConfig.ItemUseParInt[0] });
                inventoryComponent.AddItemData(addItems);
            }

            // 随机宝箱
            if (itemConfig.ItemSubType == ItemSubType.SuiJiBaoXian)
            {
                inventoryComponent.RemoveItem(request.ItemId, request.Num);

                int index = RandomHelper.RandomNumber(0, itemConfig.ItemUseParInt.Length);
                int itemId = itemConfig.ItemUseParInt[index];

                List<RewardItem> addItems = new List<RewardItem>();
                addItems.Add(new RewardItem() { ItemId = itemId, ItemNum = 1 });
                inventoryComponent.AddItemData(addItems);
            }

            // 英雄经验
            if (itemConfig.ItemSubType == ItemSubType.HeroExp)
            {
                Hero hero = unit.GetComponent<HeroComponentS>().GetHero(request.HeroId);

                if (hero == null)
                {
                    response.Error = ErrorCode.ERR_NotExistHero;
                    return;
                }

                inventoryComponent.RemoveItem(request.ItemId, request.Num);

                for (int i = 0; i < request.Num; i++)
                {
                    int expValue = RandomHelper.RandomNumber(itemConfig.ItemUseParInt[0], itemConfig.ItemUseParInt[1]);
                    HeroHelper.AddHeroExp(hero, request.Num * expValue);
                }

                HeroHelper.UpdateHeroNumeric(unit, hero);
                HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Update);
            }

            // 英雄星级
            if (itemConfig.ItemSubType == ItemSubType.HeroHunshi)
            {
                Hero hero = unit.GetComponent<HeroComponentS>().GetHero(request.HeroId);

                if (hero == null)
                {
                    response.Error = ErrorCode.ERR_NotExistHero;
                    return;
                }

                inventoryComponent.RemoveItem(request.ItemId, request.Num);

                for (int i = 0; i < request.Num; i++)
                {
                    int hunShiValue = RandomHelper.RandomNumber(itemConfig.ItemUseParInt[0], itemConfig.ItemUseParInt[1]);
                    HeroHelper.AddHeroHunShi(hero, request.Num * hunShiValue);
                }

                HeroHelper.UpdateHeroNumeric(unit, hero);
                HeroHelper.UpdateHeroSkill(hero);
                HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Update);
            }

            // 英雄碎片合成英雄
            if (itemConfig.ItemSubType == ItemSubType.HeroShard)
            {
                int heroConfigId = itemConfig.ItemUseParInt[0];
                int needNum = itemConfig.ItemUseParInt[1];
                
                List<RewardItem> removeItems = new List<RewardItem>();
                removeItems.Add(new RewardItem() { ItemId = item.ConfigId, ItemNum = needNum });
                int error = inventoryComponent.RemoveItemData(removeItems);
                
                if (error != ErrorCode.ERR_Success)
                {
                    response.Error = error;
                    return;
                }

                unit.GetComponent<HeroComponentS>().AddHeroByConfigId(heroConfigId);
            }

            await ETTask.CompletedTask;
        }
    }
}