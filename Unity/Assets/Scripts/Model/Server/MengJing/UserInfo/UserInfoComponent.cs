using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class UserInfoComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public string Account;
        public long UnitId;
        public long AccInfoID;
        public long RobotId { get; set; }

        public string PlayerName;
        public long Gold;
        public long Diamond;
        public long Exp;
        public int Lv;
        public int RechargeNumber;

        [BsonIgnore]
        public readonly M2C_RoleDataBroadcast m2C_RoleDataBroadcast = M2C_RoleDataBroadcast.Create();
    }
}