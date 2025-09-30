using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(UserInfoComponentS))]
    [MessageLocationHandler(SceneType.Map)]
    public class C2M_GMCommandHandler : MessageLocationHandler<Unit, C2M_GMCommand>
    {
        protected override async ETTask Run(Unit unit, C2M_GMCommand message)
        {
            try
            {
                string[] commands = message.GMMsg.Split('#');
                if (commands.Length == 0)
                {
                    return;
                }

                switch (int.Parse(commands[0]))
                {
                    case 1: //新增道具1#12000003#200 【添加道具/道具id/道具数量】
                    {
                        int itemId = int.Parse(commands[1]);
                        int itemNum = int.Parse(commands[2]);

                        List<RewardItem> rewardItems = new List<RewardItem>();
                        rewardItems.Add(new RewardItem() { ItemId = itemId, ItemNum = itemNum });
                        unit.GetComponent<InventoryComponentS>().AddItemData(rewardItems, InventoryContainerType.Bag);
                        break;
                    }
                    case 2: //新增英雄
                    {
                        int heroId = int.Parse(commands[1]);
                        
                        unit.GetComponent<HeroComponentS>().AddHeroByConfigId(heroId);
                        break;
                    }

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
            }

            await ETTask.CompletedTask;
        }
    }
}