using Unity.Mathematics;

namespace ET.Client
{
    public class Skill_RangedBasicAttack : SkillHandlerC
    {
        public override void OnInit(SkillC skill)
        {
            skill.Speed = skill.SkillConfig.Speed;
            skill.NowPosition = skill.TheUnitFrom.Position;
        }

        public override void OnExecute(SkillC skill)
        {
            skill.PlaySkillEffects(skill.TheUnitFrom.Position);
        }

        public override void OnUpdate(SkillC skill, float deltaTime)
        {
            if (skill.TheUnitTarget == null)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }

            float3 direction = skill.TheUnitTarget.Position - skill.NowPosition;
            float distanceToTarget = math.length(direction);
            float moveStep = skill.Speed * deltaTime;

            if (distanceToTarget <= moveStep)
            {
                skill.SkillState = SkillState.Finished;
                skill.NowPosition = skill.TheUnitTarget.Position;
            }
            else
            {
                skill.NowPosition += math.normalize(direction) * moveStep;
            }
            
            if (skill.SkillConfig.SkillEffectID != 0)
            {
                EventSystem.Instance.Publish(skill.Root(), new SkillEffectMove()
                {
                    EffectInstanceId = skill.EffectInstanceId[0],
                    Unit = skill.TheUnitFrom,
                    Postion = skill.NowPosition,
                    Angle = direction.x
                });
            }
        }

        public override void OnFinished(SkillC skill)
        {
            skill.EndSkillEffect();
        }
    }
}