using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class AchievementComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public List<int> ReceivedAchievementRewardIds = new();

        [BsonIgnore]
        public List<EntityRef<Achievement>> AchievementList = new();
    }
}