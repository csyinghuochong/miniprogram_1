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

            // 搜索最近的敌人
            Unit closestEnemy = null;
            float closestDistance = float.MaxValue;

            foreach (EntityRef<Unit> unitRef in unitComponent.GetAll())
            {
                Unit u = unitRef;
                if (myUnit.IsCanAttackUnit(u))
                {
                    float dist = math.distance(myUnit.Position, u.Position);
                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestEnemy = u;
                    }
                }
            }

            // 更新目标为最近的敌人
            if (closestEnemy != null)
            {
                aiComponent.TargetId = closestEnemy.Id;
            }
            else
            {
                aiComponent.TargetId = 0;
            }

            if (aiComponent.TargetId == 0)
            {
                return 1;
            }

            Unit target = aiComponent.Scene().GetComponent<UnitComponent>().Get(aiComponent.TargetId);
            if (target == null)
            {
                aiComponent.TargetId = 0;
                return 1;
            }

            // 不在攻击距离内追击敌人
            float distance = math.distance(target.Position, aiComponent.GetParent<Unit>().Position);
            bool zhuiji = distance > aiComponent.ActDistance;

            return zhuiji ? 0 : 1;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Unit unit = aiComponent.GetParent<Unit>();

            for (int i = 0; i < 10000; i++)
            {
                Unit target = unit.GetParent<UnitComponent>().Get(aiComponent.TargetId);
                if (target != null)
                {
                    float currentDistance = math.distance(unit.Position, target.Position);
                    if (currentDistance > aiComponent.ActDistance)
                    {
                        float3 ttt;
                        if (currentDistance < 8)
                        {
                            // 近距离站分散点
                            float3 dir = unit.Position - target.Position;
                            float ange = math.degrees(math.atan2(dir.x, dir.y));
                            // 将ID散列到360°，使连续ID也能均匀分布
                            float addg = (unit.Id % 8) * 45f;
                            quaternion rotation = quaternion.Euler(0, 0, math.radians(ange + addg));
                            ttt = target.Position + math.mul(rotation, math.up()) * (aiComponent.ActDistance - 0.1f);
                        }
                        else
                        {
                            float3 direction = math.normalize(target.Position - unit.Position);
                            ttt = unit.Position + direction * (currentDistance - (aiComponent.ActDistance - 0.1f));
                        }

                        MoveHelper.PathResultTo(unit, ttt);
                    }
                    else
                    {
                        unit.Stop();
                    }
                }

                await aiComponent.Root().GetComponent<TimerComponent>().WaitAsync(300, cancellationToken);
                if (cancellationToken.IsCancel())
                {
                    return;
                }
            }
        }
    }
}