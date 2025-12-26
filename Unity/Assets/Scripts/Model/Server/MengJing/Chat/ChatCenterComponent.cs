using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class ChatCenterComponent : Entity, IAwake, IDestroy, IDeserialize
    {
        [BsonIgnore]
        public Dictionary<string, EntityRef<ChatRoom>> ChatRoomDict { get; set; } = new();
    }
}