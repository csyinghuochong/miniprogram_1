namespace ET.Server
{
    [FriendOf(typeof(ChatUnitComponent))]
    [EntitySystemOf(typeof(ChatUnitComponent))]
    public static partial class ChatUnitComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ChatUnitComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ChatUnitComponent self)
        {
        }
    }
}