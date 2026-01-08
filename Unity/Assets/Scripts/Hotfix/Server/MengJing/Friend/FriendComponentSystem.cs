namespace ET.Server
{
    [EntitySystemOf(typeof(FriendComponent))]
    [FriendOf(typeof(FriendComponent))]
    public static partial class FriendComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FriendComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FriendComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this FriendComponent self)
        {
        }
    }
}