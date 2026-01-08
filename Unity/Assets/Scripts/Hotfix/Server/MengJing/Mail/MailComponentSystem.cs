namespace ET.Server
{
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailComponent))]
    [EntitySystemOf(typeof(MailComponent))]
    public static partial class MailComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MailComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MailComponent self)
        {
            self.MailList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this MailComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Mail mail)
                {
                    self.MailList.Add(mail);
                }
            }
        }

        public static void AddMail(this MailComponent self, MailInfo mailInfo)
        {
            Mail mail = self.AddChildWithId<Mail>(mailInfo.Id);
            mail.FromMessage(mailInfo);

            self.MailList.Add(mail);
        }

        public static void Check(this MailComponent self)
        {
            for (int i = self.MailList.Count - 1; i >= 0; i--)
            {
                Mail mail = self.MailList[i];
                if (mail.MailDeleteState == (int)MailDeleteState.Deleted || mail.EndTime < TimeHelper.ServerNow())
                {
                    mail.Dispose();
                    self.MailList.RemoveAt(i);
                }
            }
        }
    }
}