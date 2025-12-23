namespace ET.Client
{
    public static class ClientFriendHelper
    {
        public static async ETTask<int> GetAllFriend(Scene root)
        {
            C2Friend_GetAllFriend request = C2Friend_GetAllFriend.Create();

            Friend2C_GetAllFriend response = (Friend2C_GetAllFriend)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                FriendComponentC friendComponentC = root.GetComponent<FriendComponentC>();
                friendComponentC.Clear();

                foreach (FriendInfo friendInfo in response.FriendList)
                {
                    friendComponentC.AddFriendFromMessage(friendInfo);
                }

                foreach (FriendInfo friendInfo in response.RequestList)
                {
                    friendComponentC.AddRequestFromMessage(friendInfo);
                }

                foreach (FriendInfo friendInfo in response.BlackList)
                {
                    friendComponentC.AddBlackFromMessage(friendInfo);
                }
            }

            return response.Error;
        }

        public static async ETTask<int> FriendRequest(Scene root, long unitId)
        {
            C2Friend_FriendRequest request = C2Friend_FriendRequest.Create();
            request.UnitId = unitId;

            Friend2C_FriendRequest response = (Friend2C_FriendRequest)await root.GetComponent<ClientSenderComponent>().Call(request);

            return response.Error;
        }

        public static async ETTask<int> FriendRequestAccept(Scene root, long unitId, int isAgree)
        {
            C2Friend_FriendRequestAccept request = C2Friend_FriendRequestAccept.Create();
            request.UnitId = unitId;
            request.IsAgree = isAgree;

            Friend2C_FriendRequestAccept response = (Friend2C_FriendRequestAccept)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                FriendComponentC friendComponentC = root.GetComponent<FriendComponentC>();
                friendComponentC.FriendRequestAccept(unitId, isAgree);
            }

            return response.Error;
        }
    }
}