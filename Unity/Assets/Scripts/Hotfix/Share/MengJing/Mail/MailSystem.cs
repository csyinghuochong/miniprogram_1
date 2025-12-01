namespace ET
{
    [EntitySystemOf(typeof(Mail))]
    [FriendOf(typeof(Mail))]
    [FriendOf(typeof(MailRewardComponent))]
    public static partial class MailSystem
    {
        [EntitySystem]
        private static void Awake(this Mail self)
        {
            self.AddComponent<MailRewardComponent>();
        }

        [EntitySystem]
        private static void Destroy(this Mail self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this Mail self)
        {
            self.AddComponent<MailRewardComponent>();
        }

        public static MailInfo ToMessage(this Mail self)
        {
            MailInfo mailInfo = MailInfo.Create();
            mailInfo.Title = self.Title;
            mailInfo.Content = self.Content;
            mailInfo.Time = self.Time;
            mailInfo.DeleteTime = self.DeleteTime;
            mailInfo.MailReadState = self.MailReadState;
            mailInfo.MailRewardState = self.MailRewardState;
            mailInfo.MailDeleteState = self.MailDeleteState;

            MailRewardComponentInfo mailRewardComponentInfo = MailRewardComponentInfo.Create();
            foreach (Item item in self.GetComponent<MailRewardComponent>().ItemList)
            {
                mailRewardComponentInfo.ItemInfoList.Add(item.ToMessage());
            }

            mailInfo.MailRewardComponentInfo = mailRewardComponentInfo;

            return mailInfo;
        }

        public static void FromMessage(this Mail self, MailInfo mailInfo)
        {
            self.Content = mailInfo.Content;
            self.Title = mailInfo.Title;
            self.Time = mailInfo.Time;
            self.DeleteTime = mailInfo.DeleteTime;
            self.MailReadState = mailInfo.MailReadState;
            self.MailRewardState = mailInfo.MailRewardState;
            self.MailDeleteState = mailInfo.MailDeleteState;

            MailRewardComponent mailRewardComponent = self.GetComponent<MailRewardComponent>();
            mailRewardComponent.Clear();
            foreach (ItemInfo itemInfo in mailInfo.MailRewardComponentInfo.ItemInfoList)
            {
                Item item = mailRewardComponent.AddChildWithId<Item>(itemInfo.Id);
                item.FromMessage(itemInfo);
                mailRewardComponent.ItemList.Add(item);
            }
        }
    }
}