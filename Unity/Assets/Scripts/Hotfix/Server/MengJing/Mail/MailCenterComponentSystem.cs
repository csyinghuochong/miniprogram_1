namespace ET.Server
{
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailCenterComponent))]
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

        // 等全服广播停服通知的时候才调用
        public static async ETTask SaveToDatabase(this MailCenterComponent self)
        {
            await UnitCacheHelper.SaveComponent(self.Root(), self);
        }
    }
}