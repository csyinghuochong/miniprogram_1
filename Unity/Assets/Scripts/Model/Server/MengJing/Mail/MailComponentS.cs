using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(MailUnit))]
    public class MailComponentS : Entity, IAwake, IDestroy, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<Mail>> MailList = new();
    }
}