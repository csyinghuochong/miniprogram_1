using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 对一个圆形范围内敌人造成持续伤害
    /// </summary>
    public class Skill_Circle : SkillHandler
    {
        public override void OnInit(Skill skill, Unit theUnitFrom)
        {
            skill.BaseOnInit(skill.SkillInfo, theUnitFrom);
            skill.IntervalTime = 0.5f;
        }

        public override void OnExecute(Skill skill)
        {
            skill.InitSelfBuff();
            skill.PlaySkillEffects();
            skill.TheUnitFrom.GetComponent<GameObjectComponent>().GameObject.GetComponent<SkeletonAnimation>().AnimationName = "Attack";
        }

        public override void OnUpdate(Skill skill)
        {
            skill.BaseOnUpdate();

            skill.LogTime += Time.deltaTime;

            if (skill.LogTime >= skill.IntervalTime)
            {
                skill.LogTime = 0;

                Collider[] colliders = Physics.OverlapSphere(skill.TargetPosition, 2f);

                foreach (var collider in colliders)
                {
                    if (collider.CompareTag("Monster"))
                    {
                        UnitComponent unitComponent = skill.Scene().GetComponent<UnitComponent>();
                        Unit target = unitComponent.Get(collider.GetComponent<UnitId>().Id);
                        target?.GetComponent<NumericComponentC>().ApplyChange(NumericType.Now_Hp, -skill.SkillConfig.DamgeValue);
                        return;
                    }
                }
            }
        }

        public override void OnFinished(Skill skill)
        {
            skill.EndSkillEffect();
        }

        public override void OnEffectLoaded(Skill skill)
        {
            GlobalComponent globalComponent = skill.Root().GetComponent<GlobalComponent>();
            skill.EffectGameObject.transform.SetParent(globalComponent.Unit);
            skill.EffectGameObject.transform.position = skill.TargetPosition;
        }
    }
}