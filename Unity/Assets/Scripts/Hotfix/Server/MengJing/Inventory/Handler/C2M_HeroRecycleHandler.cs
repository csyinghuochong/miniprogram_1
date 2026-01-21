using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(HeroComponent))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_HeroRecycleHandler : MessageLocationHandler<Unit, C2M_HeroRecycle, M2C_HeroRecycle>
    {
        protected override async ETTask Run(Unit unit, C2M_HeroRecycle request, M2C_HeroRecycle response)
        {
            HeroComponent heroComponent = unit.GetComponent<HeroComponent>();
            InventoryComponent inventoryComponent = unit.GetComponent<InventoryComponent>();

            List<EntityRef<Hero>> heroList = new();
            foreach (var heroId in request.HeroIdList)
            {
                Hero hero = heroComponent.GetHero(heroId);

                if (hero == null)
                {
                    response.Error = ErrorCode.ERR_ModifyData;
                    return;
                }

                heroList.Add(hero);
            }

            List<RewardItem> rewardItemList = CommonHelp.GetRecycleItems(heroList);

            foreach (Hero hero in heroList)
            {
                // 英雄上阵不能分解
                if (heroComponent.Formation.Contains(hero.Id))
                {
                    response.Error = ErrorCode.ERR_HeroInFormation;
                    return;
                }

                // 英雄有装备不能分解
                foreach (long equipId in hero.Equipments.Values)
                {
                    if (equipId != 0)
                    {
                        response.Error = ErrorCode.ERR_HeroHaveEquipment;
                        return;
                    }
                }
            }

            foreach (long heroId in request.HeroIdList)
            {
                heroComponent.RemoveHero(heroId);
            }

            inventoryComponent.AddItemData(rewardItemList);

            await ETTask.CompletedTask;
        }
    }
}