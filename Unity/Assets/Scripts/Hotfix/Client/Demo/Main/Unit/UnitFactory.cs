using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current, NumericType.Now_Hp)]
    public class NumericWatcher_UnitIsDeath : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            if (unit.Type != UnitType.Monster && unit.Type != UnitType.Hero)
            {
                return;
            }

            if (args.NewValue <= 0)
            {
                unit.GetParent<UnitComponent>().Remove(unit.Id);
            }
        }
    }

    public static partial class UnitFactory
    {
        public static Unit CreateUnit(Scene currentScene, UnitInfo unitInfo, bool mainHero = false)
        {
            bool selfpet = false;
            bool mainScene = currentScene.Name.Equals(StringBuilderData.MainCity);

            if (unitInfo.Type == UnitType.Npc)
            {
                selfpet = true;
            }

            if (mainScene && (SettingData.NoShowOther || UnitHelper.GetUnitList(currentScene, UnitType.Player).Count >= SettingData.NoShowPlayer)
                && !mainHero && !selfpet)
            {
                return null;
            }

            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitInfo.UnitId, (int)unitInfo.ConfigId);
            unitComponent.Add(unit);
            unit.MainHero = mainHero;
            unit.Type = unitInfo.Type;
            unit.ConfigId = unitInfo.ConfigId;

            unit.AddComponent<ObjectWait>();
            unit.AddComponent<MoveComponent>();

            unit.Position = unitInfo.Position;
            unit.Forward = unitInfo.Forward;

            NumericComponentC numericComponentC = unit.AddComponent<NumericComponentC>();
            foreach (var kv in unitInfo.KV)
            {
                numericComponentC.ApplyValue(kv.Key, kv.Value, false);
            }

            if (unitInfo.MoveInfo != null && unitInfo.MoveInfo.Points.Count > 0)
            {
                using (ListComponent<float3> list = ListComponent<float3>.Create())
                {
                    list.Add(unit.Position);
                    list.AddRange(unitInfo.MoveInfo.Points);

                    unit.MoveToAsync(list).Coroutine();
                }
            }

            OnAfterCreateUnit(unit);
            return unit;
        }

        public static Unit CreateHero(Scene currentScene, int heroConfigId)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(IdGenerater.Instance.GenerateId(), heroConfigId);
            unit.Type = UnitType.Hero;
            unit.ConfigId = heroConfigId;
            unitComponent.Add(unit);

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(heroConfigId);

            NumericComponentC numericComponentC = unit.AddComponent<NumericComponentC>();
            numericComponentC.ApplyValue(NumericType.Now_Hp, heroConfig.Hp);
            numericComponentC.ApplyValue(NumericType.Now_MaxHp, heroConfig.Hp);

            OnAfterCreateUnit(unit);
            return unit;
        }

        public static Unit CreateMonster(Scene currentScene, int monsterConfigId)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.AddChildWithId<Unit, int>(IdGenerater.Instance.GenerateId(), monsterConfigId);
            unit.Type = UnitType.Monster;
            unit.ConfigId = monsterConfigId;
            unitComponent.Add(unit);

            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterConfigId);

            NumericComponentC numericComponentC = unit.AddComponent<NumericComponentC>();
            numericComponentC.ApplyValue(NumericType.Now_Hp, monsterConfig.Hp);
            numericComponentC.ApplyValue(NumericType.Now_MaxHp, monsterConfig.Hp);

            OnAfterCreateUnit(unit);
            return unit;
        }

        public static Unit CreateDropItem(Scene currentScene, UnitInfo unitInfo)
        {
            UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
            long unitId = unitInfo.UnitId == 0 ? IdGenerater.Instance.GenerateId() : unitInfo.UnitId;
            if (unitComponent.Get(unitId) != null)
            {
                return null;
            }

            Unit unit = unitComponent.AddChildWithId<Unit, int>(unitId, 1);
            unit.Type = UnitType.DropItem;
            unitComponent.Add(unit);

            NumericComponentC numericComponentC = unit.AddComponent<NumericComponentC>();
            foreach (var kv in unitInfo.KV)
            {
                numericComponentC.ApplyValue(kv.Key, kv.Value, false);
            }

            unit.Position = unitInfo.Position;

            OnAfterCreateUnit(unit);
            return unit;
        }

        public static void OnAfterCreateUnit(this Unit unit)
        {
            if (!ConfigData.LoadSceneFinished)
            {
                unit.WaitLoad = true;
                return;
            }

            unit.WaitLoad = false;

            EventSystem.Instance.Publish(unit.Scene(), new AfterUnitCreate() { Unit = unit });
        }

        public static async ETTask ShowAllUnit(Scene root)
        {
            Scene curscene = root.CurrentScene();
            long instanceid = curscene.InstanceId;
            List<EntityRef<Unit>> allunits = curscene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                Unit unit = allunits[i];
                if (!unit.WaitLoad || unit.IsDisposed)
                {
                    continue;
                }

                if (unit.Type == UnitType.Player)
                {
                    await root.GetComponent<TimerComponent>().WaitFrameAsync();
                }

                if (instanceid != curscene.InstanceId)
                {
                    break;
                }

                OnAfterCreateUnit(unit);
                unit.WaitLoad = false;
            }
        }
    }
}