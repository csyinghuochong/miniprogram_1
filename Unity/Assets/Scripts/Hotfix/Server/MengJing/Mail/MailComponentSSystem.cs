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
            // 测试
            Mail mailInfoEntity1 = self.AddChild<Mail>();
            mailInfoEntity1.Title = "第一封邮件";
            mailInfoEntity1.Content = "第一封邮件的具体内容";
            self.MailInfosList.Add(mailInfoEntity1);

            Mail mailInfoEntity2 = self.AddChild<Mail>();
            mailInfoEntity2.Title = "第二封邮件";
            mailInfoEntity2.Content = "第二封邮件的具体内容";
            self.MailInfosList.Add(mailInfoEntity2);
        }

        [EntitySystem]
        private static void Destroy(this MailComponentS self)
        {
            self.MailInfosList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this MailComponentS self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Mail mail)
                {
                    self.MailInfosList.Add(mail);
                }
            }
        }
    }
}