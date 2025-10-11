using UnityEngine;

namespace ET.Client
{
    public class Skill_Action_Common : SkillHandler
    {
        public override void OnInit(Skill skill, Unit theUnitFrom)
        {
            skill.BaseOnInit(skill.SkillInfo, theUnitFrom);
        }

        public override void OnExecute(Skill skill)
        {
            skill.InitSelfBuff();
            skill.PlaySkillEffects();
        }

        public override void OnUpdate(Skill skill)
        {
            skill.BaseOnUpdate();
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

        public override void OnTriggerEnter(Skill skill)
        {
        }

        public override void OnTriggerStay(Skill skill)
        {
        }

        public override void OnTriggerExit(Skill skill)
        {
        }
    }
}