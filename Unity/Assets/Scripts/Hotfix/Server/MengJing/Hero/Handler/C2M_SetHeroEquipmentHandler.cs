using System.Collections.Generic;

namespace ET.Server
{
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_SetHeroEquipmentHandler : MessageLocationHandler<Unit, C2M_SetHeroEquipment, M2C_SetHeroEquipment>
    {
        protected override async ETTask Run(Unit unit, C2M_SetHeroEquipment request, M2C_SetHeroEquipment response)
        {
            HeroComponentS heroComponent = unit.GetComponent<HeroComponentS>();
            InventoryComponentS inventoryComponent = unit.GetComponent<InventoryComponentS>();

            Hero hero = heroComponent.GetHero(request.HeroId);
            if (hero == null)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            Item item = inventoryComponent.GetItem(request.ItemId);
            if (item == null)
            {
                response.Error = ErrorCode.ERR_ModifyData;
                return;
            }

            // 穿戴装备
            if (request.OpType == 0)
            {
                if (item.ContainerType != (int)InventoryContainerType.Bag)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(item.ConfigId);

                if (itemConfig.ItemType != (int)ItemType.Equipment)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                EquipSlotType equipSlotType = CommonHelp.GetCanEquipSlot(hero.Equipments, (ItemEquipmentType)itemConfig.ItemSubType);

                if (equipSlotType == EquipSlotType.None)
                {
                    response.Error = ErrorCode.ERR_HeroNotEquipSlot;
                    return;
                }

                // 卸下原有装备
                if (hero.Equipments[(int)equipSlotType] != 0)
                {
                    Item oldItem = inventoryComponent.GetItem(hero.Equipments[(int)equipSlotType]);
                    if (oldItem != null)
                    {
                        inventoryComponent.MoveItemToContainer(oldItem, InventoryContainerType.Bag);
                    }

                    hero.Equipments[(int)equipSlotType] = 0;
                }

                hero.Equipments[(int)equipSlotType] = item.Id;
                inventoryComponent.MoveItemToContainer(item, InventoryContainerType.HeroEquipment);

                HeroHelper.UpdateHeroNumeric(unit, hero);
                HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Update);
            }

            // 卸下装备
            if (request.OpType == 1)
            {
                if (item.ContainerType != (int)InventoryContainerType.HeroEquipment)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                int slot = 0;
                foreach (KeyValuePair<int, long> heroEquipment in hero.Equipments)
                {
                    if (heroEquipment.Value == item.Id)
                    {
                        slot = heroEquipment.Key;
                        break;
                    }
                }

                if (slot == 0)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                hero.Equipments[slot] = 0;
                inventoryComponent.MoveItemToContainer(item, InventoryContainerType.Bag);

                HeroHelper.UpdateHeroNumeric(unit, hero);
                HeroHelper.SyncHeroInfo(unit, hero, HeroOpType.Update);
            }

            await ETTask.CompletedTask;
        }
    }
}