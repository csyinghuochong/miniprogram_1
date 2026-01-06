namespace ET.Server
{
    [EntitySystemOf(typeof(ArchiveComponentS))]
    [FriendOf(typeof(ArchiveComponentS))]
    public static partial class ArchiveComponentSSystem
    {
        [EntitySystem]
        private static void Awake(this ArchiveComponentS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ArchiveComponentS self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ArchiveComponentS self)
        {
        }
    }
}