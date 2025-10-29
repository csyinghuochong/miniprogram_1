using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponentS))]
    public class M2M_UnitTransferRequestHandler : MessageHandler<Scene, M2M_UnitTransferRequest, M2M_UnitTransferResponse>
    {
        protected override async ETTask Run(Scene scene, M2M_UnitTransferRequest request, M2M_UnitTransferResponse response)
        {
            UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            Unit unit = MongoHelper.Deserialize<Unit>(request.Unit);
            unitComponent.AddChild(unit);
            unitComponent.Add(unit);
            foreach (byte[] bytes in request.Entitys)
            {
                try
                {
                    Entity entity = MongoHelper.Deserialize<Entity>(bytes);
                    unit.AddComponent(entity);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            unit.AddComponent<MoveComponent>();

            unit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);
            unit.GetComponent<DBSaveComponent>().Activeted();

            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();

            M2C_StartSceneChange m2CStartSceneChange = new()
            {
                SceneInstanceId = scene.InstanceId, 
                SceneId = request.SceneId, 
                SceneType = request.SceneType, 
                Difficulty = request.Difficulty,
                ParamInfo = request.ParamInfo
            };
            MapMessageHelper.SendToClient(unit, m2CStartSceneChange);

            M2C_CreateMyUnit m2CCreateUnits = M2C_CreateMyUnit.Create();
            switch (request.SceneType)
            {
                case MapTypeEnum.MainCityScene:
                {
                    m2CCreateUnits.Unit = MapMessageHelper.CreateUnitInfo(unit);
                    MapMessageHelper.SendToClient(unit, m2CCreateUnits);

                    unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);
                    break;
                }
                case MapTypeEnum.LocalLevel:
                {
                    unit.Position = float3.zero;
                    
                    m2CCreateUnits.Unit = MapMessageHelper.CreateUnitInfo(unit);
                    MapMessageHelper.SendToClient(unit, m2CCreateUnits);
                    
                    unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);
                    
                    // 创建英雄队列
                    HeroComponentS heroComponent = unit.GetComponent<HeroComponentS>();
                    for (int i = 0; i < heroComponent.Formation.Count; i++)
                    {
                        Hero hero = heroComponent.GetHero(heroComponent.Formation[i]);
                        if (hero == null)
                        {
                            continue;
                        }

                        float2 position = i switch
                        {
                            0 => new float2(-3, 3),
                            1 => new float2(0, 3),
                            2 => new float2(3, 3),
                            3 => new float2(-3, 0),
                            4 => new float2(0, 0),
                            5 => new float2(3, 0),
                            6 => new float2(-3, -3),
                            7 => new float2(0, -3),
                            8 => new float2(3, -3),
                            _ => float2.zero
                        };

                        UnitFactory.CreateHero(scene, unit, hero, position);
                    }
                    
                    scene.GetComponent<LocalLevelComponent>().MainUnit = unit;
                    scene.GetComponent<LocalLevelComponent>().GenerateLevel();
                    break;
                }
            }

            TransferHelper.AfterTransfer(unit, request.SceneType);

            // 解锁location，可以接收发给Unit的消息
            await scene.Root().GetComponent<LocationProxyComponent>().UnLock(LocationType.Unit, unit.Id, request.OldActorId, unit.GetActorId());
        }
    }
}