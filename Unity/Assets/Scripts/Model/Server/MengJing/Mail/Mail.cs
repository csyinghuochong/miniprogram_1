namespace ET.Server
{
    [ChildOf(typeof(MailComponentS))]
    public class Mail : Entity, IAwake, IDestroy, ISerializeToEntity
    {
        public string Title;
        public string Message;
    }
}