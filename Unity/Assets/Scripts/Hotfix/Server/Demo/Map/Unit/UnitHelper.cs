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

        public static bool IsRobot(this Unit self)
        {
            return self.Type == UnitType.Player && self.GetComponent<UserInfoComponentS>().RobotId > 0;
        }

        public static void OnDead(this Unit self, Unit attack, bool nodrop = false)
        {
            self.GetComponent<MoveComponent>()?.Stop(false);
            int waitRevive = self.OnWaitRevive();

            EventSystem.Instance.Publish(self.Scene(), new UnitKillEvent()
            {
                WaitRevive = waitRevive,
                UnitAttack = attack,
                UnitDefend = self,
                NoDrop = nodrop,
            });
        }

        /// <summary>
        /// 0 不复活 1等待复活
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int OnWaitRevive(this Unit self)
        {
            return 0;
        }
    }
}