using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class RankCenterComponent : Entity, IAwake, IDestroy, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<PlayerCombatPowerRank>> PlayerCombatPowerRankList { get; set; } = new();

        [BsonIgnore]
        public List<EntityRef<AllianceRank>> AllianceRankList { get; set; } = new();
    }
}