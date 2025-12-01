using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ChildOf(typeof(MailComponentS))]
    public class Mail : Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
    {
        public int State;
        public string Title;
        public string Content;
        public long Time;
        public long DeleteTime;

        [BsonIgnore]
        public List<EntityRef<Item>> Items = new();
    }
}