using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class MailComponentC : Entity, IAwake, IDestroy
    {
        public List<EntityRef<Mail>> MailList = new();
    }
}