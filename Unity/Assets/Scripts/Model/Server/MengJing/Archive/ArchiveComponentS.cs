using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class ArchiveComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        // 已领取的图鉴奖励ID列表
        public List<long> ReceivedArchiveRewardIds = new();

        [BsonIgnore]
        public List<EntityRef<ArchiveHero>> ArchiveHeroList = new();
    }
}