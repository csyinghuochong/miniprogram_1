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

        public static int GetCurrentPoint(this AchievementComponentC self)
        {
            int point = 0;
            foreach (Achievement achievement in self.AchievementList)
            {
                if (achievement.IsCompleted != 0)
                {
                    AchievementConfig achievementConfig = AchievementConfigCategory.Instance.Get(achievement.ConfigId);
                    point += achievementConfig.RewardPoints;
                }
            }

            return point;
        }
    }
}