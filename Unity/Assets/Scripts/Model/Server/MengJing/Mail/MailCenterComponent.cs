using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class MailCenterComponent : Entity, IAwake, IDestroy, IDeserialize
    {
        [BsonIgnore]
        public List<EntityRef<ServerMail>> ServerMails = new();
    }
}