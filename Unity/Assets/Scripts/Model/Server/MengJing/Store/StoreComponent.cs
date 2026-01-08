using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class StoreComponent : Entity, IAwake, IDestroy, ITransfer, IUnitCache, IDeserialize
    {
        public long RefreshTime;
        public int RefreshNum;

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<int, int> StoreItemList = new();
    }
}