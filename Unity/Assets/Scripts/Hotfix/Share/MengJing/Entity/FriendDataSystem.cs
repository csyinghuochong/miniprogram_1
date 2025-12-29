namespace ET
{
    [EntitySystemOf(typeof(FriendData))]
    public static partial class FriendDataSystem
    {
        [EntitySystem]
        private static void Awake(this FriendData self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FriendData self)
        {
        }

        public static FriendDataInfo ToMessage(this FriendData self)
        {
            FriendDataInfo friendDataInfo = FriendDataInfo.Create();
            friendDataInfo.UnitId = self.UnitId;
            friendDataInfo.OnLine = self.OnLine;
            friendDataInfo.LastLoginTime = self.LastLoginTime;
            friendDataInfo.PlayerName = self.PlayerName;
            friendDataInfo.Lv = self.Lv;
            friendDataInfo.CombatPower = self.CombatPower;
            return friendDataInfo;
        }

        public static void FromMessage(this FriendData self, FriendDataInfo friendDataInfo)
        {
            self.UnitId = friendDataInfo.UnitId;
            self.OnLine = friendDataInfo.OnLine;
            self.LastLoginTime = friendDataInfo.LastLoginTime;
            self.PlayerName = friendDataInfo.PlayerName;
            self.Lv = friendDataInfo.Lv;
            self.CombatPower = friendDataInfo.CombatPower;
        }
    }
}