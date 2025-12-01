namespace ET.Server
{
    [FriendOf(typeof(Mail))]
    [EntitySystemOf(typeof(Mail))]
    public static partial class MailSystem
    {
        [EntitySystem]
        private static void Awake(this Mail self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Mail self)
        {
        }

        public static MailInfo ToMessage(this Mail self)
        {
             MailInfo mailInfo = MailInfo.Create();

             return mailInfo;
        }
        
        public static void FromMessage(this Mail self, MailInfo mailInfo)
        {
            self.State = mailInfo.State;
            self.Content = mailInfo.Content;
            self.Title = mailInfo.Title;
            self.Time = mailInfo.Time;
            self.DeleteTime = mailInfo.DeleteTime;
        }
    }
}