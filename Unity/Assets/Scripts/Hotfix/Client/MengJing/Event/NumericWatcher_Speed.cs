namespace ET.Client
{
    [NumericWatcher(SceneType.Current, NumericType.Now_MoveSpeed)]
    public class NumericWatcher_Speed : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            unit.GetComponent<Move2DComponent>()?.ChangeSpeed(args.NewValue / 10000f);
        }
    }
}