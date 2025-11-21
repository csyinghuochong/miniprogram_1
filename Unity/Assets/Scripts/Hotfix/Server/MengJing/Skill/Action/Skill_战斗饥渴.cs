namespace ET.Server
{
    public class Skill_战斗饥渴 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
        }

        public override void OnExecute(SkillS skill)
        {
            if (skill.TheUnitTarget == null)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }

            foreach (int id in skill.SkillConfig.BuffID)
            {
                skill.SkillBuff(id, skill.TheUnitTarget);
            }
            
            return;
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}