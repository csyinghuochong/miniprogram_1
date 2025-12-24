using System.Collections.Generic;

namespace ET.Server
{
    [MessageHandler(SceneType.Friend)]
    public class C2Friend_GetAllFriendHandler : MessageHandler<FriendUnit, C2Friend_GetAllFriend, Friend2C_GetAllFriend>
    {
        protected override async ETTask Run(FriendUnit friendUnit, C2Friend_GetAllFriend request, Friend2C_GetAllFriend response)
        {
            Scene root = friendUnit.Root();
            FriendComponentS friendComponent = friendUnit.GetComponent<FriendComponentS>();

            response.FriendList = await GetFriendInfoList(root, friendComponent.FriendList);
            response.RequestList = await GetFriendInfoList(root, friendComponent.RequestList);
            response.BlackList = await GetFriendInfoList(root, friendComponent.BlackList);

            await ETTask.CompletedTask;
        }

        private async ETTask<List<FriendDataInfo>> GetFriendInfoList(Scene root, List<long> friendUnitIdList)
        {
            List<FriendDataInfo> friendDataInfoList = new List<FriendDataInfo>();

            foreach (long unitId in friendUnitIdList)
            {
                FriendDataInfo friendDataInfo = await FriendHelper.GetFriendDataInfo(root, unitId);
                friendDataInfoList.Add(friendDataInfo);
            }

            return friendDataInfoList;
        }
    }
}