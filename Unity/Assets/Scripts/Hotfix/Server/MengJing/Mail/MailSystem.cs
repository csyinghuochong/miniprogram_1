namespace ET.Server
{
    [FriendOf(typeof(Mail))]
    [EntitySystemOf(typeof(Mail))]
    public static partial class MailSystem
    {
        [EntitySystem]
        private static void Awake(this Mail self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Mail self)
        {
        }
    }
}