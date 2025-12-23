namespace ET.Server
{
    [EntitySystemOf(typeof(FriendUnitComponent))]
    public static partial class FriendUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FriendUnitComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FriendUnitComponent self)
        {
        }
    }
}