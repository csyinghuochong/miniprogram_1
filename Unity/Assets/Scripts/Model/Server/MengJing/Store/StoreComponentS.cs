using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class StoreComponentS : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public long RefreshTime;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> StoreItemList = new();
    }
}