using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 发射一个物体，追踪敌人，到达敌人位置时造成单体伤害，如射手发射一只箭
    /// </summary>
    public class Skill_Projectile_Track : SkillHandler
    {
        public override void OnInit(Skill skill)
        {
        }

        public override void OnExecute(Skill skill)
        {
            skill.InitSelfBuff();
            skill.PlaySkillEffects();

            skill.TheUnitFrom.GetComponent<GameObjectComponent>().GameObject.GetComponent<SkeletonAnimation>().AnimationName = "attack";
        }

        public override void OnUpdate(Skill skill)
        {
            if (skill.TheUnitTarget == null)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }

            if (skill.EffectGameObject == null)
            {
                return;
            }
            
            // 向目标移动
            Transform target = skill.TheUnitTarget.GetComponent<GameObjectComponent>().GameObject.transform;
            Vector3 dir = target.position - skill.EffectGameObject.transform.position;
            skill.EffectGameObject.transform.position += dir.normalized * 2f * Time.deltaTime;
            skill.EffectGameObject.transform.forward = dir;

            // 检测是否到达目标
            if (Vector3.Distance(skill.EffectGameObject.transform.position, target.position) <= 0.5f)
            {
                skill.TheUnitTarget?.GetComponent<NumericComponentC>().ApplyChange(NumericType.Now_Hp, -skill.SkillConfig.DamgeValue);
                skill.SkillState = SkillState.Finished;
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
            skill.EffectGameObject.transform.position = skill.TheUnitFrom.GetComponent<GameObjectComponent>().GameObject.transform.position;
        }
    }
}