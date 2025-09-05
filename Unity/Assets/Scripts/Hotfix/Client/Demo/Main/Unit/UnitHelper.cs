using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

namespace ET.Client
{
    public static partial class UnitHelper
    {
        /// <summary>
        /// 是否小黑屋
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public static bool IsBackRoom(Scene root)
        {
            Unit unit = GetMyUnitFromClientScene(root);
            return unit != null ? unit.IsBackRoom() : false;
        }

        public static bool IsBackRoom(this Unit self)
        {
            return false;
        }

        public static bool IsPasture(this Unit self)
        {
            return self.Type == UnitType.Pasture;
        }

        public static bool IsMonster(this Unit self)
        {
            return self.Type == UnitType.Monster;
        }

        

        public static bool IsJingLingMonster(this Unit self)
        {
            if (self.Type != UnitType.Monster)
            {
                return false;
            }

            int sonType = MonsterConfigCategory.Instance.Get(self.ConfigId).MonsterSonType;
            return sonType == 58 || sonType == 59;
        }

        public static long GetMyUnitId(Scene root)
        {
            PlayerInfoComponent playerInfoComponent = root.GetComponent<PlayerInfoComponent>();
            return playerInfoComponent.CurrentRoleId;
        }

        public static int GetTeamDungeonXieZhu(this Unit self)
        {
            NumericComponentC numericComponent = self.GetComponent<NumericComponentC>();
            return numericComponent.GetAsInt(NumericType.TeamDungeonXieZhu);
        }

        public static int GetTeamDungeonTimes(this Unit self)
        {
            NumericComponentC numericComponent = self.GetComponent<NumericComponentC>();
            return numericComponent.GetAsInt(NumericType.TeamDungeonTimes);
        }

        public static int GetMaxPiLao(this Unit self)
        {
            return self.IsYueKaStates() ? GlobalValueConfigCategory.Instance.MaxPiLaoYuKaUser : GlobalValueConfigCategory.Instance.MaxPiLao;
        }

        public static bool IsYueKaStates(this Unit self)
        {
            return self.GetComponent<NumericComponentC>().GetAsInt(NumericType.YueKaRemainTimes) > 0;
        }

        public static bool IsYueKaEndStates(this Unit self)
        {
            NumericComponentC numericComponent = self.GetComponent<NumericComponentC>();
            return numericComponent.GetAsInt(NumericType.YueKaEndTime) > 0;
        }

        public static int GetMaoXianExp(this Unit self)
        {
            int rechargeNum = self.GetComponent<NumericComponentC>().GetAsInt(NumericType.RechargeNumber);
            rechargeNum *= 10;
            rechargeNum += self.GetComponent<NumericComponentC>().GetAsInt(NumericType.MaoXianExp);

            Log.Debug(
                $" GetMaoXianExp:  {self.GetComponent<NumericComponentC>().GetAsInt(NumericType.RechargeNumber)}   {self.GetComponent<NumericComponentC>().GetAsInt(NumericType.MaoXianExp)}");

            return rechargeNum;
        }

        public static int GetWuqiItemID(Scene root)
        {
            Unit unit = GetMyUnitFromClientScene(root);
            int itemId = unit.GetComponent<NumericComponentC>().GetAsInt(NumericType.Now_Weapon);
            return itemId;
        }

        public static List<Unit> GetUnitList(Scene currentScene, int unitType)
        {
            List<Unit> list = new List<Unit>();
            List<EntityRef<Unit>> allunits = currentScene.GetComponent<UnitComponent>().GetAll();
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

        public static Unit GetMyUnitFromClientScene(Scene root)
        {
            PlayerInfoComponent playerInfoComponent = root.GetComponent<PlayerInfoComponent>();
            Scene currentScene = root.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent>().Get(playerInfoComponent.CurrentRoleId);
        }

        public static Unit GetUnitFromZoneSceneByID(Scene zoneScene, long id)
        {
            Scene currentScene = zoneScene.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent>().Get(id);
        }

        public static Unit GetMyUnitFromCurrentScene(Scene currentScene)
        {
            PlayerInfoComponent playerInfoComponent = currentScene.Root().GetComponent<PlayerInfoComponent>();
            return currentScene.GetComponent<UnitComponent>().Get(playerInfoComponent.CurrentRoleId);
        }

        public static List<Unit> GetUnitsByType(Scene root, int unitType)
        {
            List<Unit> units = new List<Unit>();
            foreach (Unit unit in root.CurrentScene().GetComponent<UnitComponent>().GetAll())
            {
                if (unit.Type == unitType)
                {
                    units.Add(unit);
                }
            }

            return units;
        }

        public static List<Unit> GetUnitsByTypes(Scene root, List<int> unitType)
        {
            List<Unit> units = new List<Unit>();
            foreach (Unit unit in root.CurrentScene().GetComponent<UnitComponent>().GetAll())
            {
                if (unitType.Contains(unit.Type))
                {
                    units.Add(unit);
                }
            }

            return units;
        }

        public static float3 GetBornPostion(this Unit self)
        {
            NumericComponentC numericComponent = self.GetComponent<NumericComponentC>();
            return new float3(numericComponent.GetAsFloat(NumericType.Born_X),
                numericComponent.GetAsFloat(NumericType.Born_Y),
                numericComponent.GetAsFloat(NumericType.Born_Z));
        }
        
        
        public static bool IsSelfRobot(this Unit self)
        {
            // return self.Root().GetComponent<UserInfoComponentC>().UserInfo.RobotId > 0;
            return false;
        }

        public static int GetMonsterType(this Unit self)
        {
            return MonsterConfigCategory.Instance.Get(self.ConfigId).MonsterType;
        }
        
        public static int GetMonsterSonType(this Unit self)
        {
            return MonsterConfigCategory.Instance.Get(self.ConfigId).MonsterSonType;
        }

        public static bool GetMonsterShowDissolve(this Unit self)
        {
            if (self.Type!= UnitType.Monster)
            {
                return false;   
            }

            int MonsterSonType = self.GetMonsterSonType();
            return MonsterSonType <= 50;
        }
        
        public static long GetMasterId(this Unit self)
        {
            if (self.Type == UnitType.Player)
            {
                return self.Id;
            }

            if (self.Type == UnitType.Pet || self.Type == UnitType.Monster
                || self.Type == UnitType.JingLing || self.Type == UnitType.Pasture)
            {
                return self.GetComponent<NumericComponentC>().GetAsLong(NumericType.MasterId);
            }

            return 0;
        }

        public static bool IsBoss(this Unit self)
        {
            if (self.Type != UnitType.Monster)
            {
                return false;
            }

            return self.GetMonsterType() == MonsterTypeEnum.Boss;
        }

        public static bool IsYeWaiMonster(this Unit self)
        {
            return self.Type == UnitType.Monster && self.GetComponent<NumericComponentC>().GetAsLong(NumericType.MasterId) == 0;
        }

        public static int GetBattleCamp(this Unit self)
        {
            return self.GetComponent<NumericComponentC>().GetAsInt(NumericType.BattleCamp);
        }

        public static long GetTeamId(this Unit self)
        {
            return self.GetComponent<NumericComponentC>().GetAsLong(NumericType.TeamId);
        }

        public static bool IsSameTeam(this Unit self, Unit other)
        {
            if (self.Id == other.Id)
            {
                return true;
            }

            long teamid_1 = self.GetTeamId();
            long teamid_2 = other.GetTeamId();
            return teamid_1 == teamid_2 && teamid_1 != 0;
        }

        public static long GetUnionId(this Unit self)
        {
            return self.GetComponent<NumericComponentC>().GetAsLong(NumericType.UnionId_0);
        }

        public static long GetUnionLeader(this Unit self)
        {
            return self.GetComponent<NumericComponentC>().GetAsLong(NumericType.UnionLeader);
        }

        public static bool IsSameUnion(this Unit self, Unit other)
        {
            long teamid_1 = self.GetUnionId();
            long teamid_2 = other.GetUnionId();
            return teamid_1 == teamid_2 && teamid_1 != 0;
        }

        public static int GetAttackMode(this Unit self)
        {
            return self.GetComponent<NumericComponentC>().GetAsInt(NumericType.AttackMode);
        }

       
        public static List<Unit> GetUnitListByDis(Scene scene, float3 pos, int unitType, float maxdis)
        {
            List<Unit> list = new List<Unit>();
            List<EntityRef<Unit>> allunits  = scene.GetComponent<UnitComponent>().GetAll();
        
            for (int i = 0; i < allunits.Count; i++)
            {
                Unit unit = allunits[i];
                if (unit.Type != unitType)
                {
                    continue;
                }
        
                if (math.distance(pos, unit.Position) > maxdis)
                {
                    continue;
                }
                list.Add(unit);
            }
            return list;
        }
        
        public static bool IsHaveBoss(Scene scene, float3 vector3, float dis)
        {
            List<EntityRef<Unit>> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                Unit unit = allunits[i];
                if (unit.Type != UnitType.Monster)
                {
                    continue;
                }

                if (unit.IsBoss() && unit.GetComponent<NumericComponentC>().GetAsInt(NumericType.Now_Dead) == 0 &&
                    PositionHelper.Distance2D(vector3, unit.Position) < dis)
                {
                    return true;
                }
            }

            return false;
        }
    }
}