namespace ET.Server
{
    public static class FriendHelper
    {
        public static async ETTask<FriendDataInfo> GetFriendDataInfo(Scene root, long unitId)
        {
            FriendUnitComponent friendUnitComponent = root.GetComponent<FriendUnitComponent>();

            UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(root, unitId);

            FriendDataInfo friendDataInfo = FriendDataInfo.Create();
            friendDataInfo.UnitId = unitId;
            friendDataInfo.OnLine = friendUnitComponent.Children.ContainsKey(unitId) ? 1 : 0;
            friendDataInfo.PlayerName = userInfoComponent.GetPlayerName();
            friendDataInfo.Lv = userInfoComponent.GetLv();

            return friendDataInfo;
        }
    }
}