namespace ET
{
    [EntitySystemOf(typeof(FriendDate))]
    public static partial class FriendDataSystem
    {
        [EntitySystem]
        private static void Awake(this FriendDate self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FriendDate self)
        {
        }

        public static FriendDataInfo ToMessage(this FriendDate self)
        {
            FriendDataInfo friendDataInfo = FriendDataInfo.Create();
            friendDataInfo.UnitId = self.UnitId;
            friendDataInfo.OnLine = self.OnLine;
            friendDataInfo.PlayerName = self.PlayerName;
            friendDataInfo.Lv = self.Lv;
            return friendDataInfo;
        }

        public static void FromMessage(this FriendDate self, FriendDataInfo friendDataInfo)
        {
            self.UnitId = friendDataInfo.UnitId;
            self.OnLine = friendDataInfo.OnLine;
            self.PlayerName = friendDataInfo.PlayerName;
            self.Lv = friendDataInfo.Lv;
        }
    }
}