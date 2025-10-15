using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 单体伤害 在范围内对一个单位造成伤害，例如战士普通攻击
    /// </summary>
    public class Skill_SingleTarget : SkillHandler
    {
        public override void OnInit(Skill skill)
        {
            skill.DelayTime = 0.5f; //0.5秒后出伤害
            skill.HasDealtDamage = false;
        }

        public override void OnExecute(Skill skill)
        {
            skill.InitSelfBuff();
            skill.PlaySkillEffects();

            skill.TheUnitFrom.GetComponent<GameObjectComponent>().GameObject.GetComponent<SkeletonAnimation>().AnimationName = "attack";
        }

        public override void OnUpdate(Skill skill)
        {
            skill.SkillLiveTime -= Time.deltaTime;

            if (skill.SkillLiveTime <= 0)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }
            
            if (skill.HasDealtDamage)
            {
                return;
            }

            skill.DelayTime -= Time.deltaTime;

            if (skill.DelayTime <= 0)
            {
                skill.TheUnitTarget?.GetComponent<NumericComponentC>().ApplyChange(NumericType.Now_Hp, -skill.SkillConfig.DamgeValue);
                skill.HasDealtDamage = true;
            }
        }

        public override void OnFinished(Skill skill)
        {
            skill.EndSkillEffect();
        }

        public override void OnEffectLoaded(Skill skill)
        {
        }
    }
}