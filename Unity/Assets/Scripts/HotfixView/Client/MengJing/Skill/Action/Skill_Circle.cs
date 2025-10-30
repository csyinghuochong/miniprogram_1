using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 对一个圆形范围内敌人造成持续伤害
    /// </summary>
    public class Skill_Circle : SkillHandler
    {
        public override void OnInit(SkillC skillC)
        {
            skillC.IntervalTime = 0.5f;
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
            
            skillC.LogTime += Time.deltaTime;

            if (skillC.LogTime >= skillC.IntervalTime)
            {
                skillC.LogTime = 0;

                Collider[] colliders = Physics.OverlapSphere(skillC.TargetPosition, 2f);

                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("Monster"))
                    {
                        UnitComponent unitComponent = skillC.Scene().GetComponent<UnitComponent>();
                        Unit target = unitComponent.Get(collider.GetComponent<UnitId>().Id);
                        target?.GetComponent<NumericComponentC>().ApplyChange(NumericType.Now_Hp, -skillC.SkillConfig.DamgeValue);
                        return;
                    }
                }
            }
        }

        public override void OnFinished(SkillC skillC)
        {
            skillC.EndSkillEffect();
        }

        public override void OnEffectLoaded(SkillC skillC)
        {
            GlobalComponent globalComponent = skillC.Root().GetComponent<GlobalComponent>();
            skillC.EffectGameObject.transform.SetParent(globalComponent.Unit);
            skillC.EffectGameObject.transform.position = skillC.TargetPosition;
        }
    }
}