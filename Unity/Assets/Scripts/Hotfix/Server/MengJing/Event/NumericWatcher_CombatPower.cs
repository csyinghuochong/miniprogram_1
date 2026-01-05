namespace ET.Server
{
    [NumericWatcher(SceneType.Map, NumericType.CombatPower)]
    public class NumericWatcher_CombatPower : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            if (unit.Type != UnitType.Player)
            {
                return;
            }

            if (args.OldValue == args.NewValue)
            {
                return;
            }

            M2Rank_UpdatePlayerRankData request = M2Rank_UpdatePlayerRankData.Create();
            request.UnitId = unit.Id;
            request.CombatPower = args.NewValue;

            unit.Root().GetComponent<MessageLocationSenderComponent>().Get(LocationType.Rank).Call(unit.Id, request).Coroutine();
        }
    }
}