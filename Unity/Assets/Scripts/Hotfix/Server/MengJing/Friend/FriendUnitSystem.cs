namespace ET.Server
{
    [EntitySystemOf(typeof(FriendUnit))]
    [ComponentOf(typeof(FriendUnit))]
    public static partial class FriendUnitSystem
    {
        [EntitySystem]
        private static void Awake(this FriendUnit self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FriendUnit self)
        {
        }
    }
}