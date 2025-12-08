namespace ET.Client
{
    [EntitySystemOf(typeof(RelinkComponent))]
    [FriendOf(typeof(RelinkComponent))]
    public static partial class RelinkComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RelinkComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RelinkComponent self)
        {
        }
    }
}