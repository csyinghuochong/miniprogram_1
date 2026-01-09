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
        }
    }
}