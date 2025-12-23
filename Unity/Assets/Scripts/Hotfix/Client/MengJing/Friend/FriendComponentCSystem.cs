namespace ET.Client
{
    [EntitySystemOf(typeof(FriendComponentC))]
    public static partial class FriendComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this FriendComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this FriendComponentC self)
        {
        }

        public static void Clear(this FriendComponentC self)
        {
            foreach (Friend friend in self.FriendList)
            {
                friend?.Dispose();
            }

            self.FriendList.Clear();

            foreach (Friend friend in self.RequestList)
            {
                friend?.Dispose();
            }

            self.RequestList.Clear();

            foreach (Friend friend in self.BlackList)
            {
                friend?.Dispose();
            }

            self.BlackList.Clear();
        }

        public static void AddFriendFromMessage(this FriendComponentC self, FriendInfo friendInfo)
        {
            Friend friend = self.AddChild<Friend>();
            friend.FromMessage(friendInfo);
            self.FriendList.Add(friend);
        }

        public static void AddRequestFromMessage(this FriendComponentC self, FriendInfo friendInfo)
        {
            Friend friend = self.AddChild<Friend>();
            friend.FromMessage(friendInfo);
            self.RequestList.Add(friend);
        }

        public static void AddBlackFromMessage(this FriendComponentC self, FriendInfo friendInfo)
        {
            Friend friend = self.AddChild<Friend>();
            friend.FromMessage(friendInfo);
            self.BlackList.Add(friend);
        }
    }
}