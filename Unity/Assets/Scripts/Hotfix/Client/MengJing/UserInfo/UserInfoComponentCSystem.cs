namespace ET.Client
{
    [EntitySystemOf(typeof(UserInfoComponentC))]
    [FriendOf(typeof(UserInfoComponentC))]
    public static partial class UserInfoComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this UserInfoComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this UserInfoComponentC self)
        {
        }
    }
}