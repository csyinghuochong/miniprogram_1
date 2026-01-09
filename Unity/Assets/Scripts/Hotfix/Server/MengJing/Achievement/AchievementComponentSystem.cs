namespace ET.Server
{
    [FriendOf(typeof(AchievementComponent))]
    [EntitySystemOf(typeof(AchievementComponent))]
    public static partial class AchievementComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AchievementComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this AchievementComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this AchievementComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is Achievement achievement)
                {
                    self.AchievementList.Add(achievement);
                }
            }
        }

        public static void OnLogin(this AchievementComponent self)
        {
        }
    }
}