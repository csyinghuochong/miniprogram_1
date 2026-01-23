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
            self.BattlePassList.Clear();
        }

        [EntitySystem]
        private static void Deserialize(this BattlePassComponent self)
        {
            foreach (Entity entity in self.Children.Values)
            {
                if (entity is BattlePass battlePass)
                {
                    self.BattlePassList.Add(battlePass);
                }
            }
        }

        public static void OnLogin(this BattlePassComponent self)
        {
            foreach (BattlePassConfig battlePassConfig in BattlePassConfigCategory.Instance.DataList)
            {
                bool exist = false;
                foreach (BattlePass battlePass in self.BattlePassList)
                {
                    if (battlePass.ConfigId == battlePassConfig.Id)
                    {
                        exist = true;
                        break;
                    }
                }

                if (exist)
                {
                    continue;
                }

                BattlePass newBattlePass = self.AddChild<BattlePass>();
                newBattlePass.ConfigId = battlePassConfig.Id;
                self.BattlePassList.Add(newBattlePass);
            }
        }
        
        public static BattlePass GetBattlePass(this BattlePassComponent self, int configId)
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