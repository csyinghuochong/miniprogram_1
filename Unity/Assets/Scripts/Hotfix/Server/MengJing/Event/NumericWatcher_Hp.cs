namespace ET.Server
{
    [NumericWatcher(SceneType.Map, NumericType.Now_Hp)]
    public class NumericWatcher_Hp : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            NumericComponentS numericComponentDefend = unit.GetComponent<NumericComponentS>();

            Scene scene = args.Defend.Scene();
            MapComponent mapComponent = scene.GetComponent<MapComponent>();
            MapType mapType = mapComponent.MapType;
            int sceneId = mapComponent.SceneId;

            if (args.NewValue <= 0 && numericComponentDefend.GetAsInt(NumericType.Now_Dead) == 1)
            {
                return;
            }

            Unit attack = unit.GetParent<UnitComponent>().Get(args.AttackId);
            if (args.NewValue <= 0 && numericComponentDefend.GetAsInt(NumericType.Now_Dead) == 0)
            {
                if (attack == null)
                {
                    Log.Warning($"NumericWatcher_Now_Hp.args.NewValue <= 0");
                }

                unit.OnDead(attack);
            }
        }
    }
}