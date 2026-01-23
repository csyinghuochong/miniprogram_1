namespace ET.Client
{
    [FriendOf(typeof(BattlePassComponentC))]
    [EntitySystemOf(typeof(BattlePassComponentC))]
    public static partial class BattlePassComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this BattlePassComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BattlePassComponentC self)
        {
        }

        public static void Clear(this BattlePassComponentC self)
        {
            foreach (BattlePass battlePass in self.BattlePassList)
            {
                battlePass.Dispose();
            }

            self.BattlePassList.Clear();
        }

        public static void AddOrUpdateBattlePass(this BattlePassComponentC self, BattlePassInfo battlePassInfo)
        {
            foreach (BattlePass battlePass in self.BattlePassList)
            {
                if (battlePass.ConfigId == battlePassInfo.ConfigId)
                {
                    battlePass.FromMessage(battlePassInfo);
                    return;
                }
            }

            BattlePass newBattlePass = self.AddChild<BattlePass>();
            newBattlePass.FromMessage(battlePassInfo);
            self.BattlePassList.Add(newBattlePass);
        }

        public static BattlePass GetBattlePass(this BattlePassComponentC self, int configId)
        {
            foreach (BattlePass battlePass in self.BattlePassList)
            {
                if (battlePass.ConfigId == configId)
                {
                    return battlePass;
                }
            }

            return null;
        }
    }
}