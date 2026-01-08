namespace ET.Server
{
    public static class RankHelper
    {
        public static async ETTask UpdateRankData(Scene root, PlayerCombatPowerRank playerCombatPowerRank, long unitId)
        {
            UserInfoComponentS userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponentS>(root, unitId);
            NumericComponentS numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponentS>(root, unitId);

            playerCombatPowerRank.PlayerName = userInfoComponent.GetPlayerName();
            playerCombatPowerRank.CombatPower = numericComponent.GetAsLong(NumericType.CombatPower);
        }
    }
}