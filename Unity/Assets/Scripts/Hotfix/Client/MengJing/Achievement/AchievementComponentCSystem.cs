namespace ET.Client
{
    [FriendOf(typeof(AchievementComponentC))]
    [EntitySystemOf(typeof(AchievementComponentC))]
    public static partial class AchievementComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this AchievementComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this AchievementComponentC self)
        {
        }

        public static void Clear(this AchievementComponentC self)
        {
            self.ReceivedAchievementRewardIds.Clear();
            foreach (Achievement achievement in self.AchievementList)
            {
                achievement.Dispose();
            }

            self.AchievementList.Clear();
        }

        public static void AddOrUpdateAchievement(this AchievementComponentC self, AchievementInfo achievementInfo)
        {
            foreach (Achievement achievement in self.AchievementList)
            {
                if (achievement.ConfigId == achievementInfo.ConfigId)
                {
                    achievement.FromMessage(achievementInfo);
                    return;
                }
            }

            Achievement newAchievement = self.AddChild<Achievement>();
            newAchievement.FromMessage(achievementInfo);
            self.AchievementList.Add(newAchievement);
        }
    }
}