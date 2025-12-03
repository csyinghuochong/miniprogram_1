namespace ET.Client
{
    [EntitySystemOf(typeof(MailComponentC))]
    [FriendOf(typeof(MailComponentC))]
    public static partial class MailComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this MailComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MailComponentC self)
        {
        }

        public static void UpdateMail(this MailComponentC self, MailInfo mailInfo)
        {
            Mail targetMail = null;
            foreach (Mail mail in self.MailList)
            {
                if (mail.Id == mailInfo.Id)
                {
                    targetMail = mail;
                    break;
                }
            }

            if (targetMail == null)
            {
                return;
            }

            targetMail.FromMessage(mailInfo);
        }

        public static void AddMailFromMessage(this MailComponentC self, MailInfo mailInfo)
        {
            Mail mail = self.AddChildWithId<Mail>(mailInfo.Id);
            mail.FromMessage(mailInfo);
            self.MailList.Add(mail);
        }

        public static Mail GetMail(this MailComponentC self, long mailId)
        {
            foreach (Mail mail in self.MailList)
            {
                if (mail.Id == mailId)
                {
                    return mail;
                }
            }

            return null;
        }

        public static void RemoveMail(this MailComponentC self, long mailId)
        {
            Mail remove = null;
            foreach (Mail mail in self.MailList)
            {
                if (mail.Id == mailId)
                {
                    remove = mail;
                    break;
                }
            }

            if (remove == null)
            {
                return;
            }

            self.MailList.Remove(remove);
            remove?.Dispose();
        }

        public static void Clear(this MailComponentC self)
        {
            foreach (Mail mail in self.MailList)
            {
                mail?.Dispose();
            }

            self.MailList.Clear();
        }
    }
}