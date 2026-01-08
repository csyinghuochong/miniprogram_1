namespace ET.Server
{
    /// <summary>
    /// 近战普通攻击
    /// </summary>
    public class Skill_近战普通攻击 : SkillHandler
    {
        public override void OnInit(Skill skill)
        {
            NumericComponent numericComponent = skill.TheUnitFrom.GetComponent<NumericComponent>();
            skill.TriggerTime = 1 / numericComponent.GetAsFloat(NumericType.Now_AtkSpeed) * 0.8f;
        }

        public override void OnExecute(Skill skill)
        {
        }

        public override void OnUpdate(Skill skill, float deltaTime)
        {
            skill.TriggerTime -= deltaTime;

            if (skill.TriggerTime <= 0)
            {
                Function_Fight.Fight(skill.TheUnitFrom, skill.TheUnitTarget, skill);

                skill.SkillState = SkillState.Finished;
            }
        }

        public override void OnFinished(Skill skill)
        {
        }
    }
}