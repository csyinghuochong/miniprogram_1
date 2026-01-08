using Unity.Mathematics;

namespace ET.Server
{
    public class AI_Follow : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            Unit unit = aiComponent.GetParent<Unit>();
            UnitComponent unitComponent = unit.GetParent<UnitComponent>();
            long masterId = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.MasterId);
            Unit master = unitComponent.Get(masterId);
            if (master == null)
            {
                return 1;
            }

            float distanceToMaster = math.distance(unit.Position, master.Position);

            // 距离主人太远（>15米），强制跟随，放弃战斗
            if (distanceToMaster > 15f)
            {
                aiComponent.TargetId = 0;
                return 0;
            }

            // 距离主人较远（>10米），需要跟随
            if (distanceToMaster > aiComponent.FollowDistance)
            {
                return 0;
            }

            // 距离主人不远，不需要跟随
            return 1;
        }

        private static float3 GetFollowPosition(Unit unit, Unit master)
        {
            HeroComponent heroComponent = master.GetComponent<HeroComponent>();
            float3 position = heroComponent.GetHeroPosition(unit.Id);

            return master.Position + position;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Unit unit = aiComponent.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            long masterid = numericComponent.GetAsLong(NumericType.MasterId);
            Unit master = unit.GetParent<UnitComponent>().Get(masterid);

            long oldSpeed = numericComponent.GetAsLong(NumericType.Base_Speed_Base);
            long masterSpeed = master.GetComponent<NumericComponent>().GetAsLong(NumericType.Now_MoveSpeed);
            numericComponent.ApplyValue(NumericType.Base_Speed_Base, masterSpeed);

            for (int i = 0; i < 10000; i++)
            {
                int speedProp = 100;
                float distanceToMaster = math.distance(unit.Position, master.Position);

                if (distanceToMaster > 10f)
                {
                    speedProp = 150; // 距离大于10米加速
                }

                if (distanceToMaster < 7f)
                {
                    speedProp = 0; // 距离小于7米停止
                }

                if (speedProp > 0)
                {
                    float3 nextTarget = GetFollowPosition(unit, master);
                    MoveHelper.PathResultTo(unit, nextTarget);
                }

                await aiComponent.Root().GetComponent<TimerComponent>().WaitAsync(300, cancellationToken);
                if (cancellationToken.IsCancel())
                {
                    break;
                }
            }

            if (!unit.IsDisposed)
            {
                unit.GetComponent<NumericComponent>()?.ApplyValue(NumericType.Base_Speed_Base, oldSpeed);
            }
        }
    }
}