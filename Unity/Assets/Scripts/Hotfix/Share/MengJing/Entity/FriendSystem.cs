namespace ET
{
    [EntitySystemOf(typeof(Friend))]
    public static partial class FriendSystem
    {
        [EntitySystem]
        private static void Awake(this Friend self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Friend self)
        {
        }

        public static FriendInfo ToMessage(this Friend self)
        {
            FriendInfo friendInfo = FriendInfo.Create();
            friendInfo.UnitId = self.UnitId;
            friendInfo.OnLine = self.OnLine;
            friendInfo.PlayerName = self.PlayerName;
            friendInfo.Lv = self.Lv;
            return friendInfo;
        }

        public static void FromMessage(this Friend self, FriendInfo friendInfo)
        {
            self.UnitId = friendInfo.UnitId;
            self.OnLine = friendInfo.OnLine;
            self.PlayerName = friendInfo.PlayerName;
            self.Lv = friendInfo.Lv;
        }
    }
}