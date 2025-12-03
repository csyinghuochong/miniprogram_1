namespace ET
{
    public enum MailOpType
    {
        Read, //阅读
        Received, //领取
        Delete, //删除
        ReceivedAll, //领取全部
        DeleteAllRead, //删除已读(如果有道具未领取不删除)
    }

    public enum MailReadState
    {
        Unread = 0,
        Read = 1
    }

    public enum MailRewardState
    {
        NotReceived = 0,
        Received = 1
    }

    public enum MailDeleteState
    {
        Normal = 0,
        Deleted = 1
    }

    [ChildOf]
    public class Mail : Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
    {
        public string Title;
        public string Content;
        public long Time;
        public long DeleteTime;
        public int MailReadState;
        public int MailRewardState;
        public int MailDeleteState;
    }
}