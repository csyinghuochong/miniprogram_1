using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class BattlePassComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<BattlePass>> BattlePassList = new();
    }
}