using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// 远程普通攻击  例如射手射出一只箭，这个箭是追踪敌人的
    /// Speed 箭的移动速度
    /// </summary>
    public class Skill_远程普通攻击 : SkillHandler
    {
        public override void OnInit(Skill skill)
        {
            skill.Speed = skill.SkillConfig.Speed;
            skill.NowPosition = skill.TheUnitFrom.Position;
        }

        public override void OnExecute(Skill skill)
        {
        }

        public override void OnUpdate(Skill skill, float deltaTime)
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

        public override void OnFinished(Skill skill)
        {
        }
    }
}