namespace ET.Server
{
    public static class FriendHelper
    {
        public static async ETTask<FriendDataInfo> GetFriendDataInfo(Scene root, long unitId)
        {
            FriendUnitComponent friendUnitComponent = root.GetComponent<FriendUnitComponent>();

            UserInfoComponent userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponent>(root, unitId);
            NumericComponent numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponent>(root, unitId);

            FriendDataInfo friendDataInfo = FriendDataInfo.Create();
            friendDataInfo.UnitId = unitId;
            friendDataInfo.OnLine = friendUnitComponent.Children.ContainsKey(unitId) ? 1 : 0;
            friendDataInfo.LastLoginTime = numericComponent.GetAsLong(NumericType.LastLoginTime);
            friendDataInfo.PlayerName = userInfoComponent.GetPlayerName();
            friendDataInfo.Lv = userInfoComponent.GetLv();
            friendDataInfo.CombatPower = numericComponent.GetAsLong(NumericType.CombatPower);

            return friendDataInfo;
        }

        public static void FriendOnLineChange(Scene root, long unitId, int onLine)
        {
            FriendUnitComponent friendUnitComponent = root.GetComponent<FriendUnitComponent>();

            foreach (Entity entity in friendUnitComponent.Children.Values)
            {
                FriendUnit friendUnit = entity as FriendUnit;

                if (friendUnit.Id == unitId)
                {
                    continue;
                }

                FriendComponent friendComponent = friendUnit.GetComponent<FriendComponent>();

                bool notice = friendComponent.FriendList.Contains(unitId) || friendComponent.RequestList.Contains(unitId);

                if (notice)
                {
                    Friend2C_FriendOnLineChange message = Friend2C_FriendOnLineChange.Create();
                    message.UnitId = unitId;
                    message.OnLine = onLine;
                    MapMessageHelper.SendToClient(root, friendUnit.Id, message);
                }
            }
        }
    }
}