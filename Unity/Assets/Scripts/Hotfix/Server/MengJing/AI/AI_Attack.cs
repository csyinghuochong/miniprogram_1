using Unity.Mathematics;

namespace ET.Server
{
    public class AI_Attack : AAIHandler
    {
        public override int Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            Unit target = aiComponent.Scene().GetComponent<UnitComponent>().Get(aiComponent.TargetId);
            if (target == null || target.IsDisposed)
            {
                aiComponent.TargetId = 0;
                return 1;
            }

            float distance = math.distance(target.Position, aiComponent.GetParent<Unit>().Position);

            return distance > aiComponent.ActDistance ? 1 : 0;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Unit unit = aiComponent.GetParent<Unit>();
            SkillManagerComponentS skillManagerComponent = unit.GetComponent<SkillManagerComponentS>();

            for (int i = 0; i < 100000; ++i)
            {
                int skillId = aiComponent.GetActSkillId();
                if (skillId == 0)
                {
                    break;
                }

                Unit target = unit.GetParent<UnitComponent>().Get(aiComponent.TargetId);
                if (target == null || !target.IsCanBeAttack())
                {
                    aiComponent.TargetId = 0;
                }

                if (aiComponent.TargetId != 0 && target != null)
                {
                    float distance = math.distance(target.Position, aiComponent.GetParent<Unit>().Position);
                    if (distance <= aiComponent.ActDistance && skillManagerComponent.IsCanUseSkill(skillId) == ErrorCode.ERR_Success)
                    {
                        SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);
                        float3 direction = target.Position - unit.Position;
                        float angle = math.degrees(math.atan2(direction.x, direction.y));

                        skillManagerComponent.TryUseSkill(skillId, target.Id, angle, target.Position);
                    }
                }

                // 因为协程可能被中断，任何协程都要传入cancellationToken，判断如果是中断则要返回
                await aiComponent.Root().GetComponent<TimerComponent>().WaitAsync(200, cancellationToken);
                if (cancellationToken.IsCancel())
                {
                    return;
                }
            }
        }
    }
}