namespace ET.Client
{
    [EntitySystemOf(typeof(AI_HeroComponent))]
    [FriendOf(typeof(AI_HeroComponent))]
    public static partial class AI_HeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AI_HeroComponent self)
        {
        }

        [EntitySystem]
        private static void FixedUpdate(this AI_HeroComponent self)
        {
            
        }

        [EntitySystem]
        private static void Destroy(this AI_HeroComponent self)
        {
        }
    }
}