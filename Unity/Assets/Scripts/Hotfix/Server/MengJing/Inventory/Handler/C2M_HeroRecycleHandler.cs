using System.Collections.Generic;

namespace ET.Server
{
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

            foreach (long heroId in request.HeroIdList)
            {
                // 上阵的英雄怎么处理
                // 英雄身上的装备怎么处理？？
            }
            
            inventoryComponent.AddItemData(rewardItemList);

            await ETTask.CompletedTask;
        }
    }
}