namespace ET.Server
{
    [FriendOf(typeof(ActivityMonthSignInComponent))]
    [EntitySystemOf(typeof(ActivityMonthSignInComponent))]
    public static partial class ActivityMonthSignInComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ActivityMonthSignInComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ActivityMonthSignInComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ActivityMonthSignInComponent self)
        {
        }
    }
}