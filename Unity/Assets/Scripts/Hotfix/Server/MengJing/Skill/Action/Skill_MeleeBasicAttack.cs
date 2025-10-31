namespace ET.Server
{
    /// <summary>
    /// 近战普通攻击
    /// </summary>
    public class Skill_MeleeBasicAttack : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            NumericComponentS numericComponent = skill.TheUnitFrom.GetComponent<NumericComponentS>();
            skill.TriggerTime = 1 / numericComponent.GetAsFloat(NumericType.Now_AtkSpeed) * 0.8f;
        }

        public override void OnExecute(SkillS skill)
        {
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
            skill.TriggerTime -= deltaTime;

            if (skill.TriggerTime <= 0)
            {
                Function_Fight.Fight(skill.TheUnitTarget, skill.TheUnitFrom, skill);

                skill.SkillState = SkillState.Finished;
            }
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}