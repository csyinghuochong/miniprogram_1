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

        public static int GetBattleCamp(this Unit self)
        {
            NumericComponentS numericComponent = self.GetComponent<NumericComponentS>();
            return numericComponent.GetAsInt(NumericType.BattleCamp);
        }

        public static bool IsTeam(this Unit self, Unit other)
        {
            if (self.Id == other.Id)
            {
                return false;
            }

            return self.GetBattleCamp() == other.GetBattleCamp();
        }

        public static void OnDead(this Unit self, Unit attack, bool nodrop = false)
        {
            // self.GetComponent<MoveComponent>()?.Stop(false);
            int waitRevive = self.OnWaitRevive();

            EventSystem.Instance.Publish(self.Scene(), new UnitKillEvent()
            {
                WaitRevive = waitRevive,
                UnitAttack = attack,
                UnitDefend = self,
                NoDrop = nodrop,
            });
        }

        // 0不复活 1等待复活
        public static int OnWaitRevive(this Unit self)
        {
            return 0;
        }

        public static bool IsCanAttackUnit(this Unit self, Unit defend, bool checkDead = true)
        {
            if (self.Id == defend.Id)
            {
                return false;
            }

            // 玩家不能被攻击
            if (defend.Type == UnitType.Player)
            {
                return false;
            }

            if (self.IsTeam(defend))
            {
                return false;
            }

            if (!defend.IsCanBeAttack(checkDead))
            {
                return false;
            }

            MapComponent mapComponent = self.Scene().GetComponent<MapComponent>();

            if (mapComponent.MapType != MapType.LocalLevel)
            {
                return false;
            }

            return true;
        }

        public static bool IsCanBeAttack(this Unit self, bool checkDead = true)
        {
            if (checkDead)
            {
                NumericComponentS numericComponent = self.GetComponent<NumericComponentS>();
                if (numericComponent.GetAsLong(NumericType.Now_Hp) <= 0 || numericComponent.GetAsLong(NumericType.Now_Dead) == 1)
                {
                    return false;
                }
            }

            return true;
        }

        public static int IsCanMove(this Unit self)
        {
            StateComponentS stateComponent = self.GetComponent<StateComponentS>();

            if (stateComponent.StateTypeGet(StateType.Stun))
            {
                return ErrorCode.ERR_Stun;
            }

            if (stateComponent.StateTypeGet(StateType.Freeze))
            {
                return ErrorCode.ERR_Freeze;
            }

            return ErrorCode.ERR_Success;
        }

        public static void AddAnger(this Unit self, int value)
        {
            if (value <= 0)
            {
                return;
            }
        }

        public static void AddAngerByPer(this Unit self, float value)
        {
        }
    }
}