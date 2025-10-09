using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class HeroComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        [BsonIgnore]
        public Dictionary<long, EntityRef<Hero>> Heros = new();

        public int CurrentFormationIndex;
        public List<long> Formation_1 = new();
        public List<long> Formation_2 = new();
    }
}