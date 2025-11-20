namespace ET.Server
{
    public class Skill_神之力量: SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
        }

        public override void OnExecute(SkillS skill)
        {
            foreach (int id in skill.SkillConfig.InitBuffID)
            {
                skill.SkillBuff(id, skill.TheUnitFrom);
            }
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}