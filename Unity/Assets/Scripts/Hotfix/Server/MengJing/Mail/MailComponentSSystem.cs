namespace ET.Server
{
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailComponentS))]
    [EntitySystemOf(typeof(MailComponentS))]
    public static partial class MailComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this MailComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MailComponentS self)
        {
            self.MailList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this MailComponentS self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Mail mail)
                {
                    self.MailList.Add(mail);
                }
            }
        }

        public static void AddMail(this MailComponentS self, MailInfo mailInfo)
        {
            Mail mail = self.AddChildWithId<Mail>(mailInfo.Id);
            mail.FromMessage(mailInfo);

            self.MailList.Add(mail);
        }

        public static void Check(this MailComponentS self)
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