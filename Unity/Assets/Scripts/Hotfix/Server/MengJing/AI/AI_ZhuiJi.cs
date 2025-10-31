using System.Collections.Generic;
using Unity.Mathematics;

namespace ET.Server
{
    public class AI_ZhuiJi : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            Unit myUnit = aiComponent.GetParent<Unit>();
            UnitComponent unitComponent = myUnit.GetParent<UnitComponent>();

            foreach (EntityRef<Unit> unitRef in unitComponent.GetAll())
            {
                Unit u = unitRef;
                if (u.IsCanAttackUnit(myUnit))
                {
                    aiComponent.TargetID = u.Id;
                    break;
                }
            }

            if (aiComponent.TargetID == 0 || aiComponent.IsRetreat)
            {
                return 1;
            }

            Unit target = aiComponent.Scene().GetComponent<UnitComponent>().Get(aiComponent.TargetID);
            if (target == null)
            {
                aiComponent.TargetID = 0;
                return 1;
            }

            // 不再攻击距离内追击敌人
            float distance = math.distance(target.Position, aiComponent.GetParent<Unit>().Position);
            bool zhuiji = distance >= aiComponent.ActDistance;

            return zhuiji ? 0 : 1;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Unit unit = aiComponent.GetParent<Unit>();

            long checkTime = 200;

            for (int i = 0; i < 10000; i++)
            {
                Unit target = unit.GetParent<UnitComponent>().Get(aiComponent.TargetID);
                if (target != null)
                {
                    bool zhuiji = math.distance(unit.Position, target.Position) >= aiComponent.ActDistance;
                    if (!zhuiji)
                    {
                        unit.Stop(-2);
                    }

                    M2C_PathfindingResult m2CPathfindingResult = M2C_PathfindingResult.Create();
           
                    MoveComponent  moveComponent = unit.GetComponent<MoveComponent>();
                    List<float3> position = new List<float3>();
                    position.Add(unit.Position);
                    position.Add(target.Position);
            
                    MapMessageHelper.Broadcast(unit, m2CPathfindingResult);
                    MoveHelper.PathResultToAsync(unit, position, moveComponent).Coroutine();
                }

                await aiComponent.Root().GetComponent<TimerComponent>().WaitAsync(checkTime, cancellationToken);
                if (cancellationToken.IsCancel())
                {
                    return;
                }
            }
        }
    }
}