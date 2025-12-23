namespace ET.Server
{
    [EntitySystemOf(typeof(FriendComponentS))]
    [FriendOf(typeof(FriendComponentS))]
    public static partial class FriendComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this FriendComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FriendComponentS self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this FriendComponentS self)
        {
        }
    }
}