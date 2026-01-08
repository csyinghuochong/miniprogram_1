namespace ET.Server
{
    /// <summary>
    /// 近战普通攻击
    /// </summary>
    public class Skill_近战普通攻击 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
            NumericComponent numericComponent = skill.TheUnitFrom.GetComponent<NumericComponent>();
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
                Function_Fight.Fight(skill.TheUnitFrom, skill.TheUnitTarget, skill);

                skill.SkillState = SkillState.Finished;
            }
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}