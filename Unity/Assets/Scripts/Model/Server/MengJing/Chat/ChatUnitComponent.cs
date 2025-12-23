using System.Collections.Generic;

namespace ET.Server
{
    [ComponentOf(typeof(Scene))]
    public class ChatUnitComponent : Entity, IAwake, IDestroy
    {
    }
}