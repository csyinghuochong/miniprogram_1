using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    public static partial class UnitFactory
    {
        public static async ETTask<Unit> LoadUnit(Player player, Scene scene, CreateRoleInfo createRoleInfo, string account, long accountId)
        {
            Unit unit = await UnitCacheHelper.GetUnitCache(scene, player.UnitId);

            bool isNewUnit = unit == null;

            // if (isNewUnit)
            // {
            //     unit = await UnitFactory.Create(scene, player.UnitId, UnitType.Player,createRoleInfo,account, accountId);
            //
            //     UnitCacheHelper.AddOrUpdateUnitAllCache(unit);
            // }

            await Create(scene, unit, player.UnitId, UnitType.Player, createRoleInfo, account, accountId);

            //UnitCacheHelper.AddOrUpdateUnitAllCache(unit);

            return unit;
        }

        public static async ETTask Create(Scene scene, Unit unit, long id, int unitType, CreateRoleInfo createRoleInfo, string account, long accountId)
        {
            await ETTask.CompletedTask;
            
            //UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
            switch (unitType)
            {
                case UnitType.Player:
                {
                    //Unit unit = unitComponent.AddChildWithId<Unit, int>(id, 1001);
                    unit.AddComponent<MoveComponent>();
                    unit.Position = new float3(-10, 0, -10);
                    unit.Type = UnitType.Player;
                    unit.ConfigId = createRoleInfo.PlayerOcc;
                    if (unit.GetComponent<UserInfoComponentS>() == null)
                    {
                        UserInfoComponentS userInfoComponentS = unit.AddComponent<UserInfoComponentS>();
                        userInfoComponentS.Account = account;
                        userInfoComponentS.UnitId = id;
                        userInfoComponentS.AccInfoID = accountId;
                        userInfoComponentS.PlayerName = createRoleInfo.PlayerName;
                    }

                    if (unit.GetComponent<NumericComponentS>() == null)
                    {
                        NumericComponentS numericComponentS = unit.AddComponent<NumericComponentS>();
                        numericComponentS.ApplyValue(NumericType.Now_Speed, 60000, false); // 速度是6米每秒
                        numericComponentS.ApplyValue(NumericType.AOI, 15000, false); // 视野15米
                    }

                    unit.AddDataComponent<DBSaveComponent>();
                    
                    //unitComponent.Add(unit);
                    // 加入aoi
                    unit.AddComponent<AOIEntity, int, float3>(9 * 1000, unit.Position);
                    return;
                }
                default:
                    throw new Exception($"not such unit type: {unitType}");
            }
        }
    }
}