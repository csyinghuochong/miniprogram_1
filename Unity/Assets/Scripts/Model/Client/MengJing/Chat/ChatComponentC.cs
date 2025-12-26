using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ChatComponentC : Entity, IAwake, IDestroy
    {
        public Dictionary<string, EntityRef<ChatRoom>> ChatRoomDict { get; set; } = new();
    }
}