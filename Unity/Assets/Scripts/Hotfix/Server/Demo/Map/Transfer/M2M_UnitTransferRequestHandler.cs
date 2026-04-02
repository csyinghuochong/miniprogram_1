using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;

namespace ET.Server
{
    [MessageHandler(SceneType.Map)]
    [FriendOf(typeof(HeroComponent))]
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

            unit.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.OrderedMessage);
            unit.GetComponent<DBSaveComponent>().Activeted();

            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            M2C_StartSceneChange m2CStartSceneChange = new()
            {
                SceneInstanceId = scene.InstanceId,
                SceneId = request.SceneId,
                MapType = request.MapType,
                TimeScale = scene.TimeScale
            };
            MapMessageHelper.SendToClient(unit, m2CStartSceneChange);

            M2C_CreateMyUnit m2CCreateUnits = M2C_CreateMyUnit.Create();
            switch (request.MapType)
            {
                case (int)MapType.MainCity:
                {
                    float x = numericComponent.GetAsFloat(NumericType.MainCity_X);
                    float y = numericComponent.GetAsFloat(NumericType.MainCity_Y);
                    float z = numericComponent.GetAsFloat(NumericType.MainCity_Z);
                    unit.Position = new float3(x, y, z);

                    m2CCreateUnits.Unit = MapMessageHelper.CreateUnitInfo(unit);
                    MapMessageHelper.SendToClient(unit, m2CCreateUnits);

                    unit.AddComponent<AOIEntity, int, float3>(40 * 1000, unit.Position);
                    // unit.AddComponent<TransformNoticeToClientComponent>();
                    unit.AddComponent<PathfindingComponent, byte[]>(NavmeshComponent.Instance.Get(CommonHelp.GetMapObjName(MapType.LocalLevel)));
                    unit.AddComponent<Move2DComponent>();
                    break;
                }
                case (int)MapType.LocalLevel:
                {
                    unit.Position = float3.zero;

                    numericComponent.ApplyValue(NumericType.BattleCamp, (int)CampType.CampPlayer_1, false);
                    numericComponent.ApplyValue(NumericType.BattleMode, 1, false);
                    numericComponent.ApplyValue(NumericType.PassedLevelId, 0, false);

                    m2CCreateUnits.Unit = MapMessageHelper.CreateUnitInfo(unit);
                    MapMessageHelper.SendToClient(unit, m2CCreateUnits);

                    unit.AddComponent<AOIEntity, int, float3>(40 * 1000, unit.Position);
                    // unit.AddComponent<TransformNoticeToClientComponent>();
                    unit.AddComponent<PathfindingComponent, byte[]>(NavmeshComponent.Instance.Get(CommonHelp.GetMapObjName(MapType.LocalLevel)));
                    unit.AddComponent<Move2DComponent>();

                    // 测试一直从第一关开始
                    scene.GetComponent<LocalLevelComponent>().MainUnit = unit;
                    scene.GetComponent<LocalLevelComponent>().GenerateLevel();
                    break;
                }
            }

            TransferHelper.AfterTransfer(unit, (MapType)request.MapType);

            // 解锁location，可以接收发给Unit的消息
            await scene.Root().GetComponent<LocationProxyComponent>().UnLock(LocationType.Unit, unit.Id, request.OldActorId, unit.GetActorId());
        }
    }
}