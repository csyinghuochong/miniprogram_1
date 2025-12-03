using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    public enum MailReceiveType
    {
        PlayerId = 1, //玩家Id
        All = 2, //全服
        LessLv = 3, //小于30级
    }

    [ChildOf(typeof(MailCenterComponent))]
    public class ServerMail : Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
    {
        public int MailReceiveType;
        public string Params;
        public List<long> ReceivedPlayerIds = new();

        [BsonIgnore]
        public EntityRef<Mail> Mail;
    }
}