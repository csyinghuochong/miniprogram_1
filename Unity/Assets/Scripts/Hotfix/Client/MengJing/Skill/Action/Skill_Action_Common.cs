// using UnityEngine;
//
// namespace ET.Client
// {
//     public class Skill_Action_Common : SkillHandlerC
//     {
//         public override void OnInit(SkillC skillC)
//         {
//         }
//
//         public override void OnExecute(SkillC skillC)
//         {
//             skillC.InitSelfBuff();
//             skillC.PlaySkillEffects();
//         }
//
//         public override void OnUpdate(SkillC skillC)
//         {
//             skillC.SkillLiveTime -= Time.deltaTime;
//
//             if (skillC.SkillLiveTime <= 0)
//             {
//                 skillC.SkillState = SkillState.Finished;
//                 return;
//             }
//         }
//
//         public override void OnFinished(SkillC skillC)
//         {
//             skillC.EndSkillEffect();
//         }
//
//         public override void OnEffectLoaded(SkillC skillC)
//         {
//             // GlobalComponent globalComponent = skill.Root().GetComponent<GlobalComponent>();
//             // skill.EffectGameObject.transform.SetParent(globalComponent.Unit);
//             // skill.EffectGameObject.transform.position = skill.TargetPosition;
//             // skill.EffectGameObject.transform.localRotation = Quaternion.Euler(0, skill.SkillInfo.TargetAngle, 0);
//
//             // ColliderCallback colliderCallback = skill.EffectGameObject.GetComponent<ColliderCallback>();
//             // colliderCallback.OnTriggerEnterAction = (Collider) => { this.OnTriggerEnter(skill); };
//             // colliderCallback.OnTriggerStayAction = (Collider) => { this.OnTriggerStay(skill); };
//             // colliderCallback.OnTriggerExitAction = (Collider) => { this.OnTriggerExit(skill); };
//         }
//
//         public void OnTriggerEnter(SkillC skillC)
//         {
//         }
//
//         public void OnTriggerStay(SkillC skillC)
//         {
//         }
//
//         public void OnTriggerExit(SkillC skillC)
//         {
//         }
//     }
// }