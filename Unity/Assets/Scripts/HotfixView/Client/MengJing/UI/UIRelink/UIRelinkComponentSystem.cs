namespace ET.Client
{
    [EntitySystemOf(typeof(UIRelinkComponent))]
    [FriendOf(typeof(UIRelinkComponent))]
    public static partial class UIRelinkComponentSystem
    {
        [EntitySystem]
        private static void Awake(this UIRelinkComponent self)
        {
        }
    }
}