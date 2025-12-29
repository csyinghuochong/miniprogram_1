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
            foreach (FriendData friendData in self.FriendList)
            {
                friendData?.Dispose();
            }

            self.FriendList.Clear();

            foreach (FriendData friendData in self.RequestList)
            {
                friendData?.Dispose();
            }

            self.RequestList.Clear();

            foreach (FriendData friendData in self.BlackList)
            {
                friendData?.Dispose();
            }

            self.BlackList.Clear();
        }

        public static void AddFriendFromMessage(this FriendComponentC self, FriendDataInfo friendDataInfo)
        {
            FriendData friendData = self.AddChild<FriendData>();
            friendData.FromMessage(friendDataInfo);
            self.FriendList.Add(friendData);
        }

        public static void AddRequestFromMessage(this FriendComponentC self, FriendDataInfo friendDataInfo)
        {
            FriendData friendData = self.AddChild<FriendData>();
            friendData.FromMessage(friendDataInfo);
            self.RequestList.Add(friendData);
        }

        public static void AddBlackFromMessage(this FriendComponentC self, FriendDataInfo friendDataInfo)
        {
            FriendData friendData = self.AddChild<FriendData>();
            friendData.FromMessage(friendDataInfo);
            self.BlackList.Add(friendData);
        }

        public static void FriendRequestAccept(this FriendComponentC self, long unitId, int isAgree)
        {
            FriendData friendData = null;
            foreach (FriendData data in self.RequestList)
            {
                if (data.UnitId == unitId)
                {
                    friendData = data;
                    break;
                }
            }

            if (friendData == null)
            {
                return;
            }

            self.RequestList.Remove(friendData);

            if (isAgree == 1)
            {
                self.FriendList.Add(friendData);
            }
            else
            {
                friendData.Dispose();
            }
        }

        public static void DeleteFriend(this FriendComponentC self, long unitId)
        {
            for (int i = self.FriendList.Count - 1; i >= 0; i--)
            {
                FriendData friendData = self.FriendList[i];
                if (friendData.UnitId == unitId)
                {
                    friendData.Dispose();
                    self.FriendList.RemoveAt(i);
                    return;
                }
            }
        }

        public static void DeleteRequest(this FriendComponentC self, long unitId)
        {
            for (int i = self.RequestList.Count - 1; i >= 0; i--)
            {
                FriendData friendData = self.RequestList[i];
                if (friendData.UnitId == unitId)
                {
                    friendData.Dispose();
                    self.RequestList.RemoveAt(i);
                    return;
                }
            }
        }

        public static void DeleteBlack(this FriendComponentC self, long unitId)
        {
            for (int i = self.BlackList.Count - 1; i >= 0; i--)
            {
                FriendData friendData = self.BlackList[i];
                if (friendData.UnitId == unitId)
                {
                    friendData.Dispose();
                    self.BlackList.RemoveAt(i);
                    return;
                }
            }
        }

        public static void FriendOnLineChange(this FriendComponentC self, long unitId, int onLine)
        {
            foreach (FriendData data in self.FriendList)
            {
                if (data.UnitId == unitId)
                {
                    data.OnLine = onLine;
                    return;
                }
            }

            foreach (FriendData data in self.RequestList)
            {
                if (data.UnitId == unitId)
                {
                    data.OnLine = onLine;
                    return;
                }
            }
        }

        public static bool IsFriend(this FriendComponentC self, long unitId)
        {
            foreach (FriendData friendData in self.FriendList)
            {
                if (friendData.UnitId == unitId)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsBlack(this FriendComponentC self, long unitId)
        {
            foreach (FriendData friendData in self.BlackList)
            {
                if (friendData.UnitId == unitId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}