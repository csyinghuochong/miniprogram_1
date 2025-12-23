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

        private async ETTask<List<FriendInfo>> GetFriendInfoList(Scene root, List<long> friendUnitIdList)
        {
            FriendUnitComponent friendUnitComponent = root.GetComponent<FriendUnitComponent>();

            List<FriendInfo> friendInfoList = new List<FriendInfo>();

            foreach (long unitId in friendUnitIdList)
            {
                UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(root, unitId);

                FriendInfo friendInfo = FriendInfo.Create();
                friendInfo.UnitId = unitId;
                friendInfo.OnLine = friendUnitComponent.Children.ContainsKey(unitId) ? 1 : 0;
                friendInfo.PlayerName = userInfoComponent.GetPlayerName();
                friendInfo.Lv = userInfoComponent.GetLv();

                friendInfoList.Add(friendInfo);
            }

            return friendInfoList;
        }
    }
}