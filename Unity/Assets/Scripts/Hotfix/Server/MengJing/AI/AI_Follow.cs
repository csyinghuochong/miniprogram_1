using Unity.Mathematics;

namespace ET.Server
{
    public class AI_Follow : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            Unit unit = aiComponent.GetParent<Unit>();
            UnitComponent unitComponent = unit.GetParent<UnitComponent>();
            long masterId = unit.GetComponent<NumericComponentS>().GetAsLong(NumericType.MasterId);
            Unit master = unitComponent.Get(masterId);
            if (master == null)
            {
                return 1;
            }

            float distance = math.distance(unit.Position, master.Position);
            if (distance > aiComponent.FollowDistance)
            {
                aiComponent.TargetID = 0;
                return 0;
            }

            if (unitComponent.Get(aiComponent.BeAttackId) != null)
            {
                aiComponent.TargetID = aiComponent.BeAttackId;
                return 1;
            }

            return aiComponent.TargetID != 0 ? 1 : 0;
        }

        private static float3 GetFollowPosition(Unit unit, Unit master)
        {
            HeroComponentS heroComponent = master.GetComponent<HeroComponentS>();
            float3 position = heroComponent.GetHeroPosition(unit.Id);

            return master.Position + position;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Unit unit = aiComponent.GetParent<Unit>();
            NumericComponentS numericComponentS = unit.GetComponent<NumericComponentS>();
            long masterid = numericComponentS.GetAsLong(NumericType.MasterId);
            Unit master = unit.GetParent<UnitComponent>().Get(masterid);

            long oldSpeed = numericComponentS.GetAsLong(NumericType.Base_Speed_Base);

            long masterSpeed = master.GetComponent<NumericComponentS>().GetAsLong(NumericType.Now_MoveSpeed);
            numericComponentS.ApplyValue(NumericType.Base_Speed_Base, masterSpeed);

            for (int i = 0; i < 10000; i++)
            {
                int speedProp = 100;
                float distacne = math.distance(unit.Position, master.Position);

                if (distacne > 10f) //距离大于加速追
                {
                    speedProp = 150;
                }

                if (distacne < 7f) //距离小于停止追
                {
                    
                    speedProp = 0;
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
                unit.GetComponent<NumericComponentS>()?.ApplyValue(NumericType.Base_Speed_Base, oldSpeed);
            }
        }
    }
}