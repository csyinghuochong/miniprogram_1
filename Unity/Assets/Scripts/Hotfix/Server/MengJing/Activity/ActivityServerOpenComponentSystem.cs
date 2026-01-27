namespace ET.Server
{
    [FriendOf(typeof(ActivityServerOpenComponent))]
    [EntitySystemOf(typeof(ActivityServerOpenComponent))]
    public static partial class ActivityServerOpenComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ActivityServerOpenComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ActivityServerOpenComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ActivityServerOpenComponent self)
        {
        }
    }
}