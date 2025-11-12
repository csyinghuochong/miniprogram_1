using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class HeroComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<Hero>> Heros = new();

        public List<long> Formation = new();
    }
}