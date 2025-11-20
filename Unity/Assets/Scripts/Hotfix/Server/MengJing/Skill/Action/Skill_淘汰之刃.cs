namespace ET.Server
{
    /// <summary>
    /// GameObjectParameter 35000,3000,40000,5000 对敌人造成350%物理伤害，如果敌人生命值低于30%，造成400%物理伤害；若成功击杀，将额外恢复50%的怒气。
    /// </summary>
    public class Skill_淘汰之刃 : SkillHandlerS
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
            if (skill.TheUnitTarget == null)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }

            NumericComponentS defendNumeric = skill.TheUnitTarget.GetComponent<NumericComponentS>();
            int nowHp = defendNumeric.GetAsInt(NumericType.Now_Hp);
            int maxHp = defendNumeric.GetAsInt(NumericType.Now_MaxHp);
            if (nowHp * 1f / maxHp > skill.SkillConfig.GameObjectParameter[1] / 10000f)
            {
                Function_Fight.Fight(skill.TheUnitFrom, skill.TheUnitTarget, skill, skill.SkillConfig.GameObjectParameter[0] / 10000f);
            }
            else
            {
                Function_Fight.Fight(skill.TheUnitFrom, skill.TheUnitTarget, skill, skill.SkillConfig.GameObjectParameter[2] / 10000f);
            }

            // 击杀恢复怒气
            if (defendNumeric.GetAsInt(NumericType.Now_Dead) == 1)
            {
                Log.Warning("淘汰之刃 击杀目标恢复怒气");
            }

            skill.SkillState = SkillState.Finished;
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}