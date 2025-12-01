namespace ET
{
    [FriendOf(typeof(MailRewardComponent))]
    [EntitySystemOf(typeof(MailRewardComponent))]
    public static partial class MailRewardComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MailRewardComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MailRewardComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this MailRewardComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Item item)
                {
                    self.ItemList.Add(item);
                }
            }
        }
    }
}