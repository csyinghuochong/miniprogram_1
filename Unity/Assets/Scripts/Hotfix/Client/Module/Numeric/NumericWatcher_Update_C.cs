using System;

namespace ET.Client
{
    [NumericWatcher(SceneType.Current, NumericType.Now_Dead)]
    public class NumericWatcher_Now_Dead : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            // if (args.NewValue == 0) //复活
            // {
            //     unit.Position = unit.GetBornPostion();
            //     EventSystem.Instance.Publish(args.Defend.Root(), new UnitRevive() { Unit = args.Defend });
            // }
            //
            // if (args.NewValue == 1) //死亡
            // {
            //     unit.GetComponent<HeroDataComponentC>().OnDead();
            //     EventSystem.Instance.Publish(args.Defend.Root(), new UnitDead() { Unit = args.Defend, Wait =  true});
            // }
        }
    }

    [NumericWatcher(SceneType.Current, NumericType.Now_Speed)]
    public class NumericWatcher_Now_Speed : INumericWatcher
    {
        public void Run(Unit unit, NumbericChange args)
        {
            float speed = args.Defend.GetComponent<NumericComponentC>().GetAsFloat(NumericType.Now_Speed);
            args.Defend.GetComponent<MoveComponent>().ChangeSpeed(speed);
        }
    }

}