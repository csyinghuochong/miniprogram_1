using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum ChatRoomType
    {
        World = 0, //世界 
        Alliance = 1, //联盟
        Private = 2, //私聊
    }

    public enum ChatRoomState
    {
        Open = 0,
        Close = 1,
    }

    [ChildOf]
    public class ChatRoom : Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
    {
        public string ChatRoomKey { get; set; }
        public int ChatRoomType { get; set; }
        public int ChatRoomState { get; set; }
        public List<long> UnitList { get; set; } = new();

        [BsonIgnore]
        public List<EntityRef<Chat>> ChatList { get; set; } = new();
    }
}