using System;
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
                    float3 targetPos = target.Position;
                    if (aiComponent.ActDistance < 5)
                    {
                        float range = 90f;
                        int maxCount = 8;

                        // XOR 让不同 target 的结果差异更大
                        int slot = (int)((unit.InstanceId ^ target.InstanceId) % maxCount);
                        bool right = slot % 2 == 0;

                        int sideIndex = slot / 2;
                        int sideCount = maxCount / 2;
                        float angle = (range / sideCount) * sideIndex - (range / 2f);

                        if (!right)
                        {
                            angle = angle >= 0 ? 180f - angle : -(180f + angle);
                        }

                        float rad = math.radians(angle);
                        float3 offset = new float3(math.cos(rad), math.sin(rad), 0) * (aiComponent.ActDistance - 0.1f);
                        targetPos = target.Position + offset;
                    }
                    else
                    {
                        float3 direction = math.normalize(targetPos - unit.Position);
                        targetPos = unit.Position + direction * (currentDistance - (aiComponent.ActDistance - 0.1f));
                    }

                    MoveHelper.PathResultTo(unit, targetPos);
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