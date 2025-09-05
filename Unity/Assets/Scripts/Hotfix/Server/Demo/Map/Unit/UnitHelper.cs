using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof(MoveComponent))]
    [FriendOf(typeof(NumericComponentS))]
    public static partial class UnitHelper
    {
        // 获取看见unit的玩家，主要用于广播
        public static Dictionary<long, EntityRef<AOIEntity>> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetBeSeePlayers();
        }

        public static Dictionary<long, EntityRef<AOIEntity>> GetGetSeeUnits(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetSeeUnits();
        }

        public static Unit GetUnitByCellIndex(Scene curScene, int cellIndex, List<Unit> allunits)
        {
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].GetComponent<NumericComponentS>().GetAsInt(NumericType.GatherCellIndex) == cellIndex)
                {
                    return allunits[i];
                }
            }

            return null;
        }

        public static void AddDataComponent<K>(this Unit self) where K : Entity, IAwake, new()
        {
            if (self.GetComponent<K>() == null)
            {
                self.AddComponent<K>();
            }
        }

        public static List<Unit> GetUnitList(Scene scene, int unitType)
        {
            List<Unit> list = new List<Unit>();
            List<EntityRef<Unit>> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                Unit unit = allunits[i];
                if (unit.Type == unitType)
                {
                    list.Add(allunits[i]);
                }
            }

            return list;
        }

        public static List<Unit> GetUnitList(Scene scene, List<int> unitType)
        {
            List<Unit> list = new List<Unit>();
            List<EntityRef<Unit>> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                Unit unit = allunits[i];
                if (unitType.Contains(unit.Type))
                {
                    list.Add(allunits[i]);
                }
            }

            return list;
        }

        public static List<Unit> GetUnitList(Scene scene, float3 position, int unitType, float distance)
        {
            List<Unit> units = new List<Unit>();
            List<EntityRef<Unit>> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                Unit unit = allunits[i];
                if (unit.Type != unitType)
                {
                    continue;
                }

                if (math.distance(unit.Position, position) > distance)
                {
                    continue;
                }

                units.Add(allunits[i]);
            }

            return units;
        }
    }
}