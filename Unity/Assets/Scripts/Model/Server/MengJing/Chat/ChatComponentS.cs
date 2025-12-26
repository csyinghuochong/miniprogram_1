using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(ChatUnit))]
    public class ChatComponentS : Entity, IAwake, IDestroy, IDeserialize
    {
        public List<string> ChatRoomKeyList { get; set; } = new();
    }
}