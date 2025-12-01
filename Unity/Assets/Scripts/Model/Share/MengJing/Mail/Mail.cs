namespace ET
{
    public enum MailReadState
    {
        Unread = 0,
        Read = 1
    }

    public enum MailRewardState
    {
        NoReward = 0, // 无附件
        NotReceived = 1, // 有附件未领取
        Received = 2 // 已领取
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