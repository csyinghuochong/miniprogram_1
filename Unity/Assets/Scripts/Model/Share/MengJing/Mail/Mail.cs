namespace ET
{
    public enum MailOpType
    {
        Read, //阅读
        Received, //领取
        Delete, //删除
    }

    public enum MailReadState
    {
        Unread = 0,
        Read = 1
    }

    public enum MailRewardState
    {
        NotReward = 0,
        NotReceived = 1,
        Received = 2
    }

    public enum MailDeleteState
    {
        Normal = 0,
        Deleted = 1
    }

    [ChildOf]
    public class Mail : Entity, IAwake, IDestroy, ISerializeToEntity, IDeserialize
    {
        public string From;
        public string Title;
        public string Content;
        public long Time;
        public long EndTime;
        public int MailReadState;
        public int MailRewardState;
        public int MailDeleteState;
    }
}