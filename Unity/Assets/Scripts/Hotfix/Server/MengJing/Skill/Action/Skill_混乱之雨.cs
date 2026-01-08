using Unity.Mathematics;

namespace ET.Server
{
    /// <summary>
    /// GameObjectParameter 2,5 怪物Id,时间
    /// </summary>
    public class Skill_混乱之雨 : SkillHandler
    {
        public override void OnInit(Skill skill)
        {
        }

        public override void OnExecute(Skill skill)
        {
            float2 pos = new float2(skill.TheUnitFrom.Position.x, skill.TheUnitFrom.Position.y);
            Unit unit = UnitFactory.CreateZhaoHuan(skill.Scene(), (int)skill.SkillConfig.GameObjectParameter[0], pos, skill.TheUnitFrom);
            unit.AddComponent<DeathTimeComponent, float>(skill.SkillConfig.GameObjectParameter[1]);
            
            skill.SkillState = SkillState.Finished;
        }

        public override void OnUpdate(Skill skill, float deltaTime)
        {
        }

        public override void OnFinished(Skill skill)
        {
        }
    }
}