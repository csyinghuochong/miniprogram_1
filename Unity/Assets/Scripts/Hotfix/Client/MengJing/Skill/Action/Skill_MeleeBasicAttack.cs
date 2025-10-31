namespace ET.Client
{
    public class Skill_MeleeBasicAttack : SkillHandlerC
    {
        public override void OnInit(SkillC skill)
        {
        }

        public override void OnExecute(SkillC skill)
        {
            skill.PlaySkillEffects(skill.TheUnitFrom.Position);
        }

        public override void OnUpdate(SkillC skill, float deltaTime)
        {
        }

        public override void OnFinished(SkillC skill)
        {
            skill.EndSkillEffect();
        }
    }
}