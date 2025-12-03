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

                // 击杀所有怪物
                if (message.GMMsg == "#killall")
                {
                    List<EntityRef<Unit>> units = unit.GetParent<UnitComponent>().GetAll();
                    for (int i = units.Count - 1; i >= 0; i--)
                    {
                        Unit u = units[i];
                        if (u.Type != UnitType.Monster)
                        {
                            continue;
                        }

                        u.GetComponent<NumericComponentS>().ApplyChange(NumericType.Now_Hp, -1000000000, attackid: unit.Id);
                    }

                    return;
                }

                if (commands[0] == "mail")
                {
                    M2Mail_SendMail request = M2Mail_SendMail.Create();
                    request.Msg = message.GMMsg;

                    unit.Root().GetComponent<MessageSender>().Call(UnitCacheHelper.GetMailServerId(unit.Zone()), request).Coroutine();
                    
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
                    case 3: //创建怪物
                    {
                        float posX = float.Parse(commands[1]);
                        float posY = float.Parse(commands[2]);
                        int monsterId = int.Parse(commands[3]);
                        int number = int.Parse(commands[4]);
                        if (number > 100)
                        {
                            Log.Error("number > 100");
                            return;
                        }

                        for (int c = 0; c < number; c++)
                        {
                            await unit.Root().GetComponent<TimerComponent>().WaitAsync(1);
                            float2 vector2 = new float2(posX + RandomHelper.RandomNumberFloat(-1, 1), posY);
                            Unit monster = UnitFactory.CreateMonster(unit.Scene(), monsterId, vector2);
                        }

                        break;
                    }
                    case 6:
                    {
                        int lv = int.Parse(commands[1]);
                        UserInfoComponentS userInfoComponent = unit.GetComponent<UserInfoComponentS>();

                        if (lv < userInfoComponent.GetLv())
                        {
                            return;
                        }

                        if (lv > 70)
                        {
                            return;
                        }

                        userInfoComponent.ChangeRoleData(UserDataType.Lv, lv - userInfoComponent.GetLv());
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