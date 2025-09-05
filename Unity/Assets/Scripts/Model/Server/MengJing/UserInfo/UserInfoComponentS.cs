using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class UserInfoComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public string Account { get; set; }
        public long UnitId { get; set; }
        public long AccInfoID { get; set; }
        public string PlayerName { get; set; }

        [BsonIgnore]
        public readonly M2C_RoleDataBroadcast m2C_RoleDataBroadcast = M2C_RoleDataBroadcast.Create();
    }
}