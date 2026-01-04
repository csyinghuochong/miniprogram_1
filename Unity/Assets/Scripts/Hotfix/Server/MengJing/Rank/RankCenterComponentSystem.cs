namespace ET.Server
{
    [EntitySystemOf(typeof(RankCenterComponent))]
    [FriendOf(typeof(RankCenterComponent))]
    public static partial class RankCenterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this RankCenterComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this RankCenterComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this RankCenterComponent self)
        {
        }
    }
}