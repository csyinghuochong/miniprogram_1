namespace ET.Server
{
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailCenterComponent))]
    [FriendOf(typeof(ServerMail))]
    [EntitySystemOf(typeof(MailCenterComponent))]
    public static partial class MailCenterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MailCenterComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MailCenterComponent self)
        {
            self.ServerMails.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this MailCenterComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is ServerMail serverMail)
                {
                    self.ServerMails.Add(serverMail);
                }
            }
        }

        public static ServerMail CreateServerMail(this MailCenterComponent self, MailInfo mailInfo, int receiveType, string receiveParams)
        {
            ServerMail serverMail = self.AddChild<ServerMail>();
            serverMail.MailReceiveType = receiveType;
            serverMail.Params = receiveParams;

            Mail mail = serverMail.AddChildWithId<Mail>(mailInfo.Id);
            mail.FromMessage(mailInfo);
            serverMail.Mail = mail;

            self.ServerMails.Add(serverMail);

            return serverMail;
        }

        public static void RemoveServerMailAt(this MailCenterComponent self, int index)
        {
            if (index >= 0 && index < self.ServerMails.Count)
            {
                ServerMail serverMail = self.ServerMails[index];
                serverMail.Dispose();
                self.ServerMails.RemoveAt(index);
            }
        }
    }
}