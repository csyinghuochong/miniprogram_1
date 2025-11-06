using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// 远程普通攻击  例如射手射出一只箭，这个箭是追踪敌人的
    /// Speed 箭的移动速度
    /// </summary>
    public class Skill_RangedBasicAttack : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            skill.Speed = skill.SkillConfig.Speed;
            skill.NowPosition = skill.TheUnitFrom.Position;
        }

        public override void OnExecute(SkillS skill)
        {
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
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
                Function_Fight.Fight(skill.TheUnitFrom, skill.TheUnitTarget, skill);

                skill.SkillState = SkillState.Finished;
            }
            else
            {
                skill.NowPosition += math.normalize(direction) * moveStep;
            }
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}