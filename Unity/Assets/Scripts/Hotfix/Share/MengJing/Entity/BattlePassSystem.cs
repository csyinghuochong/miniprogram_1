namespace ET
{
    [EntitySystemOf(typeof(BattlePass))]
    public static partial class BattlePassSystem
    {
        [EntitySystem]
        private static void Awake(this BattlePass self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BattlePass self)
        {
        }

        public static BattlePassInfo ToMessage(this BattlePass self)
        {
            BattlePassInfo info = BattlePassInfo.Create();
            info.ConfigId = self.ConfigId;
            info.RewardReceived_1 = self.RewardReceived_1;
            info.RewardReceived_2 = self.RewardReceived_2;
            info.RewardReceived_3 = self.RewardReceived_3;

            return info;
        }

        public static void FromMessage(this BattlePass self, BattlePassInfo info)
        {
            self.ConfigId = info.ConfigId;
            self.RewardReceived_1 = info.RewardReceived_1;
            self.RewardReceived_2 = info.RewardReceived_2;
            self.RewardReceived_3 = info.RewardReceived_3;
        }
    }
}