namespace ET.Server
{
    [NumericWatcher(SceneType.Map, NumericType.Now_MoveSpeed)]
    public class NumericWatcher_Speed : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            // unit.GetComponent<Move2DComponent>()?.ChangeSpeed(args.NewValue / 10000f);
            unit.GetComponent<UnitMoveComponent>()?.ChangeSpeed(args.NewValue / 10000f);
        }
    }
}