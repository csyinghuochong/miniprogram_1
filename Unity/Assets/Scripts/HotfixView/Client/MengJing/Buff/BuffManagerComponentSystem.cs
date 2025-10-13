namespace ET.Client
{
    [EntitySystemOf(typeof(BuffManagerComponent))]
    [FriendOf(typeof(BuffManagerComponent))]
    public static partial class BuffManagerComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuffManagerComponent self)
        {
        }

        [EntitySystem]
        private static void Update(this BuffManagerComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BuffManagerComponent self)
        {
        }
    }
}