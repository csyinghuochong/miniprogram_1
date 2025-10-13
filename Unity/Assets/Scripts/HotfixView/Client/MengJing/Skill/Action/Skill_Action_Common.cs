using UnityEngine;

namespace ET.Client
{
    public class Skill_Action_Common : SkillHandler
    {
        public override void OnInit(Skill skill)
        {
        }

        public override void OnExecute(Skill skill)
        {
            skill.InitSelfBuff();
            skill.PlaySkillEffects();
        }

        public override void OnUpdate(Skill skill)
        {
            skill.SkillLiveTime -= Time.deltaTime;

            if (skill.SkillLiveTime <= 0)
            {
                skill.SkillState = SkillState.Finished;
                return;
            }
        }

        public override void OnFinished(Skill skill)
        {
            skill.EndSkillEffect();
        }

        public override void OnEffectLoaded(Skill skill)
        {
            // GlobalComponent globalComponent = skill.Root().GetComponent<GlobalComponent>();
            // skill.EffectGameObject.transform.SetParent(globalComponent.Unit);
            // skill.EffectGameObject.transform.position = skill.TargetPosition;
            // skill.EffectGameObject.transform.localRotation = Quaternion.Euler(0, skill.SkillInfo.TargetAngle, 0);

            // ColliderCallback colliderCallback = skill.EffectGameObject.GetComponent<ColliderCallback>();
            // colliderCallback.OnTriggerEnterAction = (Collider) => { this.OnTriggerEnter(skill); };
            // colliderCallback.OnTriggerStayAction = (Collider) => { this.OnTriggerStay(skill); };
            // colliderCallback.OnTriggerExitAction = (Collider) => { this.OnTriggerExit(skill); };
        }

        public void OnTriggerEnter(Skill skill)
        {
        }

        public void OnTriggerStay(Skill skill)
        {
        }

        public void OnTriggerExit(Skill skill)
        {
        }
    }
}