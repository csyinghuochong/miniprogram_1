namespace ET.Server
{
    [EntitySystemOf(typeof(ServerMail))]
    [FriendOf(typeof(ServerMail))]
    public static partial class ServerMailSystem
    {
        [EntitySystem]
        private static void Awake(this ServerMail self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ServerMail self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ServerMail self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Mail mail)
                {
                    self.Mail = mail;
                }
            }
        }
    }
}