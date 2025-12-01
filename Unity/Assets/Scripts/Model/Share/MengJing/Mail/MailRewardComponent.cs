using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    [ComponentOf(typeof(Mail))]
    public class MailRewardComponent : Entity, IAwake, IDestroy, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<Item>> ItemList = new();
    }
}