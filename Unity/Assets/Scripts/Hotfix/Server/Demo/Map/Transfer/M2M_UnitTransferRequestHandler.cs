using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    public class M2M_UnitTransferRequestHandler: MessageHandler<Scene, M2M_UnitTransferRequest, M2M_UnitTransferResponse>
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
            
            NumericComponentS numericComponent = unit.GetComponent<NumericComponentS>();
            
            M2C_StartSceneChange m2CStartSceneChange = new() { SceneInstanceId = scene.InstanceId, SceneId = request.SceneId, SceneType = request.SceneType, Difficulty = request.Difficulty, ParamInfo = request.ParamInfo };
            MapMessageHelper.SendToClient(unit, m2CStartSceneChange);
            
            M2C_CreateMyUnit m2CCreateUnits =  M2C_CreateMyUnit.Create();;
            switch (request.SceneType)
            {
                case MapTypeEnum.MainCityScene:
                    float last_x = numericComponent.GetAsFloat(NumericType.MainCity_X);
                    float last_y = numericComponent.GetAsFloat(NumericType.MainCity_Y);
                    float last_z = numericComponent.GetAsFloat(NumericType.MainCity_Z);
                    SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(request.SceneId);
                    if (last_x ==0f)
                    {
                        unit.Position = new float3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
                    }
                    else
                    {
                        unit.Position = new float3(last_x, last_y, last_z);
                    }
                    // 通知客户端创建My Unit
                    m2CCreateUnits.Unit = MapMessageHelper.CreateUnitInfo(unit);
                    MapMessageHelper.SendToClient(unit, m2CCreateUnits);
                    
                    break;
            }

            TransferHelper.AfterTransfer(unit, request.SceneType);

            // 解锁location，可以接收发给Unit的消息
            await scene.Root().GetComponent<LocationProxyComponent>().UnLock(LocationType.Unit, unit.Id, request.OldActorId, unit.GetActorId());
        }
    }
}