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
    }
}