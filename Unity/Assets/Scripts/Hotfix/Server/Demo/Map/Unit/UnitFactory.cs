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

            CreatePlayer(scene, unit, player.UnitId, createRoleInfo, account, accountId);

            //UnitCacheHelper.AddOrUpdateUnitAllCache(unit);

            return unit;
        }

        private static void CreatePlayer(Scene scene, Unit unit, long id, CreateRoleInfo createRoleInfo, string account, long accountId)
        {
            unit.Position = new float3(0, 0, 0);
            unit.Type = UnitType.Player;
            unit.ConfigId = createRoleInfo.PlayerOcc;

            if (unit.GetComponent<UserInfoComponentS>() == null)
            {
                UserInfoComponentS userInfoComponentS = unit.AddComponent<UserInfoComponentS>();
                userInfoComponentS.Account = account;
                userInfoComponentS.UnitId = id;
                userInfoComponentS.AccInfoID = accountId;
                userInfoComponentS.PlayerName = createRoleInfo.PlayerName;
                userInfoComponentS.Lv = 1;
            }

            if (unit.GetComponent<NumericComponentS>() == null)
            {
                NumericComponentS numericComponentS = unit.AddComponent<NumericComponentS>();
                numericComponentS.ApplyValue(NumericType.Now_MoveSpeed, 60000, false); // 速度是6米每秒
                numericComponentS.ApplyValue(NumericType.AOI, 15000, false); // 视野15米
            }

            if (unit.GetComponent<InventoryComponentS>() == null)
            {
                InventoryComponentS inventoryComponentS = unit.AddComponent<InventoryComponentS>();
            }

            if (unit.GetComponent<HeroComponentS>() == null)
            {
                HeroComponentS heroComponentS = unit.AddComponent<HeroComponentS>();
            }

            unit.AddComponent<UnitInfoComponent>();
            unit.AddComponent<MoveComponent>();
            unit.AddDataComponent<DBSaveComponent>();
            unit.AddComponent<AOIEntity, int, float3>(20 * 1000, unit.Position);
        }

        public static Unit CreateHero(Scene scene, Unit master, Hero hero, float2 position)
        {
            Unit unit = scene.GetComponent<UnitComponent>().AddChildWithId<Unit, int>(hero.Id, hero.ConfigId);
            scene.GetComponent<UnitComponent>().Add(unit);
            unit.Position = new float3(position.x, position.y, 0);
            unit.Type = UnitType.Hero;

            NumericComponentS numericComponent = unit.AddComponent<NumericComponentS>();
            foreach (KeyValuePair<int, long> keyValuePair in hero.NumericDic)
            {
                numericComponent.ApplyValue(keyValuePair.Key, keyValuePair.Value, false);
            }

            UnitInfoComponent unitInfoComponent = unit.AddComponent<UnitInfoComponent>();
            unitInfoComponent.UnitName = HeroConfigCategory.Instance.Get(hero.ConfigId).HeroName;
            unitInfoComponent.MasterName = master.GetComponent<UserInfoComponentS>().PlayerName;

            unit.AddComponent<AOIEntity, int, float3>(20 * 1000, unit.Position);

            return unit;
        }

        public static Unit CreateMonster(Scene scene, int monsterConfigId, float2 position)
        {
            Unit unit = scene.GetComponent<UnitComponent>().AddChildWithId<Unit, int>(IdGenerater.Instance.GenerateId(), monsterConfigId);
            scene.GetComponent<UnitComponent>().Add(unit);
            unit.Position = new float3(position.x, position.y, 0);
            unit.Type = UnitType.Monster;

            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterConfigId);

            NumericComponentS numericComponent = unit.AddComponent<NumericComponentS>();
            numericComponent.ApplyValue(NumericType.Now_Hp, monsterConfig.Hp, false);
            numericComponent.ApplyValue(NumericType.Base_MaxHp_Base, monsterConfig.Hp, false);

            UnitInfoComponent unitInfoComponent = unit.AddComponent<UnitInfoComponent>();
            unitInfoComponent.UnitName = monsterConfig.MonsterName;

            unit.AddComponent<AOIEntity, int, float3>(20 * 1000, unit.Position);

            return unit;
        }
    }
}