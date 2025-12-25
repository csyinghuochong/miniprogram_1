namespace ET.Server
{
    public static class FriendHelper
    {
        public static async ETTask<FriendDataInfo> GetFriendDataInfo(Scene root, long unitId)
        {
            FriendUnitComponent friendUnitComponent = root.GetComponent<FriendUnitComponent>();

            UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(root, unitId);
            NumericComponentS numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponentS>(root, unitId);

            FriendDataInfo friendDataInfo = FriendDataInfo.Create();
            friendDataInfo.UnitId = unitId;
            friendDataInfo.OnLine = friendUnitComponent.Children.ContainsKey(unitId) ? 1 : 0;
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

                FriendComponentS friendComponent = friendUnit.GetComponent<FriendComponentS>();

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