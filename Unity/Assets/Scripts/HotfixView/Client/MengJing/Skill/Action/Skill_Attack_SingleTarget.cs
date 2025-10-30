using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 单体伤害 在范围内对一个单位造成伤害，例如战士普通攻击
    /// </summary>
    public class Skill_SingleTarget : SkillHandler
    {
        public override void OnInit(SkillC skillC)
        {
            skillC.DelayTime = 0.5f; //0.5秒后出伤害
            skillC.HasDealtDamage = false;
        }

        public override void OnExecute(SkillC skillC)
        {
            skillC.InitSelfBuff();
            skillC.PlaySkillEffects();

            skillC.TheUnitFrom.GetComponent<GameObjectComponent>().GameObject.GetComponent<SkeletonAnimation>().AnimationName = "attack";
        }

        public override void OnUpdate(SkillC skillC)
        {
            skillC.SkillLiveTime -= Time.deltaTime;

            if (skillC.SkillLiveTime <= 0)
            {
                skillC.SkillState = SkillState.Finished;
                return;
            }
            
            if (skillC.HasDealtDamage)
            {
                return;
            }

            skillC.DelayTime -= Time.deltaTime;

            if (skillC.DelayTime <= 0)
            {
                skillC.TheUnitTarget?.GetComponent<NumericComponentC>().ApplyChange(NumericType.Now_Hp, -skillC.SkillConfig.DamgeValue);
                skillC.HasDealtDamage = true;
            }
        }

        public override void OnFinished(SkillC skillC)
        {
            skillC.EndSkillEffect();
        }

        public override void OnEffectLoaded(SkillC skillC)
        {
        }
    }
}