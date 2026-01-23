namespace ET.Server
{
    [FriendOf(typeof(BattlePassComponent))]
    [EntitySystemOf(typeof(BattlePassComponent))]
    public static partial class BattlePassComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BattlePassComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BattlePassComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this BattlePassComponent self)
        {
        }

        public static void OnLogin(this BattlePassComponent self)
        {
            
        }
    }
}