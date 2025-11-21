using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// GameObjectParameter 2,5 怪物Id,时间
    /// </summary>
    public class Skill_混乱之雨 : SkillHandlerS
    {
        public override void OnInit(SkillS skill)
        {
        }

        public override void OnExecute(SkillS skill)
        {
            float2 pos = new float2(skill.TheUnitFrom.Position.x, skill.TheUnitFrom.Position.y);
            Unit unit = UnitFactory.CreateMonster(skill.Scene(), (int)skill.SkillConfig.GameObjectParameter[0], pos, (CampType)skill.TheUnitFrom.GetBattleCamp());
            unit.AddComponent<DeathTimeComponent, float>(skill.SkillConfig.GameObjectParameter[1]);
            
            skill.SkillState = SkillState.Finished;
        }

        public override void OnUpdate(SkillS skill, float deltaTime)
        {
        }

        public override void OnFinished(SkillS skill)
        {
        }
    }
}