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
                friendComponentC.FriendList.Clear();
                friendComponentC.ApplyList.Clear();
                friendComponentC.BlackList.Clear();
                friendComponentC.FriendList.AddRange(response.FriendList);
                friendComponentC.ApplyList.AddRange(response.ApplyList);
                friendComponentC.BlackList.AddRange(response.BlackList);
            }

            return response.Error;
        }
    }
}