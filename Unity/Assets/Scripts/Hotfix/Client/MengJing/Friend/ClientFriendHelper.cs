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

                foreach (FriendDataInfo friendInfo in response.FriendList)
                {
                    friendComponentC.AddFriendFromMessage(friendInfo);
                }

                foreach (FriendDataInfo friendInfo in response.RequestList)
                {
                    friendComponentC.AddRequestFromMessage(friendInfo);
                }

                foreach (FriendDataInfo friendInfo in response.BlackList)
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

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

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

            EventSystem.Instance.Publish(root, new FriendUpdate());

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response.Error;
        }

        public static async ETTask<int> DeleteFriend(Scene root, long unitId)
        {
            C2Friend_DeleteFriend request = C2Friend_DeleteFriend.Create();
            request.UnitId = unitId;

            Friend2C_DeleteFriend response = (Friend2C_DeleteFriend)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                FriendComponentC friendComponentC = root.GetComponent<FriendComponentC>();
                friendComponentC.DeleteFriend(unitId);
            }

            EventSystem.Instance.Publish(root, new FriendUpdate());

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response.Error;
        }

        public static async ETTask<int> BlackFriend(Scene root, long unitId, int ope)
        {
            C2Friend_BlackFriend request = C2Friend_BlackFriend.Create();
            request.UnitId = unitId;
            request.Ope = ope;

            Friend2C_BlackFriend response = (Friend2C_BlackFriend)await root.GetComponent<ClientSenderComponent>().Call(request);

            if (response.Error == ErrorCode.ERR_Success)
            {
                FriendComponentC friendComponentC = root.GetComponent<FriendComponentC>();
                if (request.Ope == 0)
                {
                    friendComponentC.DeleteRequest(unitId);
                    friendComponentC.AddBlackFromMessage(response.FriendDataInfo);
                }
                else
                {
                    friendComponentC.DeleteBlack(unitId);
                }
            }

            EventSystem.Instance.Publish(root, new FriendUpdate());

            if (ErrorCode.ErrorTips.TryGetValue(response.Error, out string tip)) EventSystem.Instance.Publish(root, new ShowTip() { Tip = tip });

            return response.Error;
        }
    }
}