namespace ET.Server
{
    [FriendOf(typeof(MailUnitComponent))]
    [EntitySystemOf(typeof(MailUnitComponent))]
    public static partial class MailUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this MailUnitComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this MailUnitComponent self)
        {
        }
    }
}