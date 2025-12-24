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
            foreach (FriendDate friend in self.FriendList)
            {
                friend?.Dispose();
            }

            self.FriendList.Clear();

            foreach (FriendDate friend in self.RequestList)
            {
                friend?.Dispose();
            }

            self.RequestList.Clear();

            foreach (FriendDate friend in self.BlackList)
            {
                friend?.Dispose();
            }

            self.BlackList.Clear();
        }

        public static void AddFriendFromMessage(this FriendComponentC self, FriendDataInfo friendDataInfo)
        {
            FriendDate friendDate = self.AddChild<FriendDate>();
            friendDate.FromMessage(friendDataInfo);
            self.FriendList.Add(friendDate);
        }

        public static void AddRequestFromMessage(this FriendComponentC self, FriendDataInfo friendDataInfo)
        {
            FriendDate friendDate = self.AddChild<FriendDate>();
            friendDate.FromMessage(friendDataInfo);
            self.RequestList.Add(friendDate);
        }

        public static void AddBlackFromMessage(this FriendComponentC self, FriendDataInfo friendDataInfo)
        {
            FriendDate friendDate = self.AddChild<FriendDate>();
            friendDate.FromMessage(friendDataInfo);
            self.BlackList.Add(friendDate);
        }

        public static void FriendRequestAccept(this FriendComponentC self, long unitId, int isAgree)
        {
            FriendDate friendDate = null;
            foreach (FriendDate friend1 in self.RequestList)
            {
                if (friend1.UnitId == unitId)
                {
                    friendDate = friend1;
                    break;
                }
            }

            if (friendDate == null)
            {
                return;
            }

            self.RequestList.Remove(friendDate);

            if (isAgree == 1)
            {
                self.FriendList.Add(friendDate);
            }
        }
    }
}