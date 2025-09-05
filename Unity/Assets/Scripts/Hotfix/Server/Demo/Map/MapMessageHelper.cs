using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    [FriendOf(typeof (NumericComponentS))]
    [FriendOf(typeof (MoveComponent))]
    public static partial class MapMessageHelper
    {
        
          public static void GetUnitInfo(Unit sendUnit, M2C_CreateUnits createUnits)
        {
            switch (sendUnit.Type)
            {
                case UnitType.Player:
                case UnitType.JingLing:
                case UnitType.Pasture:
                case UnitType.Plant:
                case UnitType.Pet:
                case UnitType.Bullet:
                case UnitType.Npc:
                case UnitType.Stall:
                case UnitType.Monster:
                case UnitType.DropItem:
                case UnitType.Transfers:
                case UnitType.CellTransfers:
                    createUnits.Units.Add(CreateUnitInfo(sendUnit));
                    break;
                default:
                    break;
            }
        }

        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = UnitInfo.Create();
            NumericComponentS nc = unit.GetComponent<NumericComponentS>();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            unitInfo.Type = (int)unit.Type();
            unitInfo.Position = unit.Position;
            unitInfo.Forward = unit.Forward;

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent != null)
            {
                if (!moveComponent.IsArrived())
                {
                    unitInfo.MoveInfo = MoveInfo.Create();
                    unitInfo.MoveInfo.Points.Add(unit.Position);
                    for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                    {
                        float3 pos = moveComponent.Targets[i];
                        unitInfo.MoveInfo.Points.Add(pos);
                    }
                }
            }

            if (nc != null)
            {
                foreach ((int key, long value) in nc.NumericDic)
                {
                    unitInfo.KV.Add(key, value);
                }
            }

            return unitInfo;
        }
        
        
        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            M2C_CreateUnits createUnits = M2C_CreateUnits.Create();
            GetUnitInfo(sendUnit, createUnits);
            SendToClient(unit, createUnits);
        }


        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            M2C_RemoveUnits removeUnits = M2C_RemoveUnits.Create();
            removeUnits.Units.Add(sendUnit.Id);
            SendToClient(unit, removeUnits);
        }

        public static void Broadcast(Unit unit, IMessage message)
        {
            
            (message as MessageObject).IsFromPool = false;
            Dictionary<long, EntityRef<AOIEntity>> dict = unit.GetBeSeePlayers();
            // 网络底层做了优化，同一个消息不会多次序列化
            MessageLocationSenderOneType oneTypeMessageLocationType =
                    unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession);
            foreach (AOIEntity u in dict.Values)
            {
                oneTypeMessageLocationType.Send(u.Unit.Id, message);
            }
        }
        
        public static void SendToClient(Unit unit, IMessage message)
        {
            unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(unit.Id, message);
        }

        public static void SendToClient(List<Unit> units, IMessage message)
        {
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(unit.Id, message);
            }
        }

        public static void SendToClient(Scene root, long GateSessionId, IMessage message)
        {
            root.GetComponent<MessageLocationSenderComponent>().Get(LocationType.GateSession).Send(GateSessionId, message);
        }

        /// <summary>
        /// 发送协议给Actor
        /// </summary>
        public static void Send(Scene root, ActorId actorId, IMessage message)
        {
            root.GetComponent<MessageSender>().Send(actorId, message);
        }
    }
}