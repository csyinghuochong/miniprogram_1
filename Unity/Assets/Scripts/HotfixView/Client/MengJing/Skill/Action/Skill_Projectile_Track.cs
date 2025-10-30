using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 发射一个物体，追踪敌人，到达敌人位置时造成单体伤害，如射手发射一只箭
    /// </summary>
    public class Skill_Projectile_Track : SkillHandler
    {
        public override void OnInit(SkillC skillC)
        {
        }

        public override void OnExecute(SkillC skillC)
        {
            skillC.InitSelfBuff();
            skillC.PlaySkillEffects();

            skillC.TheUnitFrom.GetComponent<GameObjectComponent>().GameObject.GetComponent<SkeletonAnimation>().AnimationName = "attack";
        }

        public override void OnUpdate(SkillC skillC)
        {
            if (skillC.TheUnitTarget == null)
            {
                skillC.SkillState = SkillState.Finished;
                return;
            }

            if (skillC.EffectGameObject == null)
            {
                return;
            }
            
            // 向目标移动
            Transform target = skillC.TheUnitTarget.GetComponent<GameObjectComponent>().GameObject.transform;
            Vector3 dir = target.position - skillC.EffectGameObject.transform.position;
            skillC.EffectGameObject.transform.position += dir.normalized * 2f * Time.deltaTime;
            skillC.EffectGameObject.transform.forward = dir;

            // 检测是否到达目标
            if (Vector3.Distance(skillC.EffectGameObject.transform.position, target.position) <= 0.5f)
            {
                skillC.TheUnitTarget?.GetComponent<NumericComponentC>().ApplyChange(NumericType.Now_Hp, -skillC.SkillConfig.DamgeValue);
                skillC.SkillState = SkillState.Finished;
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
            skillC.EffectGameObject.transform.position = skillC.TheUnitFrom.GetComponent<GameObjectComponent>().GameObject.transform.position;
        }
    }
}