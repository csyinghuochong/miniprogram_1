namespace ET.Server
{
    public static class RankHelper
    {
        public static async ETTask UpdateRankData(Scene root, PlayerCombatPowerRank playerCombatPowerRank, long unitId)
        {
            UserInfoComponent userInfoComponent = await UnitCacheHelper.GetComponentCache<UserInfoComponent>(root, unitId);
            NumericComponent numericComponent = await UnitCacheHelper.GetComponentCache<NumericComponent>(root, unitId);

            playerCombatPowerRank.PlayerName = userInfoComponent.GetPlayerName();
            playerCombatPowerRank.CombatPower = numericComponent.GetAsLong(NumericType.CombatPower);
        }
    }
}