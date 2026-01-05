namespace ET.Server
{
    public static class RankHelper
    {
        public static async ETTask UpdateRankData(Scene root, RankData rankData, long unitId)
        {
            UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(root, unitId);
            NumericComponentS numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponentS>(root, unitId);

            rankData.PlayerName = userInfoComponent.GetPlayerName();
            rankData.CombatPower = numericComponent.GetAsLong(NumericType.CombatPower);
        }
    }
}