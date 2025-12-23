using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class ChatComponent : Entity, IAwake, IDestroy
    {
        public List<EntityRef<ChatEntry>> ChatEntryList = new();
    }
}