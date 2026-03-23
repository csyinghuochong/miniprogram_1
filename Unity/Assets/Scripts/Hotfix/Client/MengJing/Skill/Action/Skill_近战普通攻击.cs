using Unity.Mathematics;

namespace ET.Client
{
    public class Skill_近战普通攻击 : SkillHandlerC
    {
        public override void OnInit(SkillC skill)
        {
            skill.NowPosition = new float3(skill.TheUnitFrom.Position.x, skill.TheUnitFrom.Position.y + ClientSkillHelper.GetCenterHigh(skill.TheUnitFrom), skill.TheUnitFrom.Position.z);
        }

        public override void OnExecute(SkillC skill)
        {
            skill.PlaySkillEffects(skill.NowPosition);
        }

        public override void OnUpdate(SkillC skill, float deltaTime)
        {
            skill.RunTime += deltaTime;
            if (skill.RunTime >= skill.SkillConfig.SkillLiveTime)
            {
                skill.SkillState = SkillState.Finished;
            }
        }

        public override void OnFinished(SkillC skill)
        {
            skill.EndSkillEffect();
        }
    }
}