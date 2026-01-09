namespace ET
{
    [EntitySystemOf(typeof(Achievement))]
    public static partial class AchievementSystem
    {
        [EntitySystem]
        private static void Awake(this Achievement self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Achievement self)
        {
        }

        public static AchievementInfo ToMessage(this Achievement self)
        {
            AchievementInfo info = AchievementInfo.Create();
            info.ConfigId = self.ConfigId;
            info.Progress = self.Progress;

            return info;
        }

        public static void FromMessage(this Achievement self, AchievementInfo info)
        {
            self.ConfigId = info.ConfigId;
            self.Progress = info.Progress;
        }
    }
}