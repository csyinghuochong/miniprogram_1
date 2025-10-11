using Spine.Unity;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(AI_MonsterComponent))]
    [FriendOf(typeof(AI_MonsterComponent))]
    public static partial class AI_MonsterComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AI_MonsterComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            GameObjectComponent gameObjectComponent = unit.GetComponent<GameObjectComponent>();
            self.GameObject = gameObjectComponent.GameObject;
            self.Rigidbody = gameObjectComponent.GameObject.GetComponent<Rigidbody>();

            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
            self.AttackRange = monsterConfig.ActDistance;
            self.MoveSpeed = (float)monsterConfig.MoveSpeed;
        }

        [EntitySystem]
        private static void Update(this AI_MonsterComponent self)
        {
            if (self.Target == null || self.Target.GetComponent<UnitId>().Id == 0)
            {
                self.GameObject.GetComponent<SkeletonAnimation>().AnimationName = "Idle";
                self.GameObject.GetComponent<SkeletonAnimation>().loop = true;
                self.FindTarget();
            }

            if (self.Target != null && self.Target.GetComponent<UnitId>().Id != 0)
            {
                float distance = Vector3.Distance(self.GameObject.transform.position, self.Target.position);

                // 如果在攻击范围内，进行攻击
                if (distance <= self.AttackRange)
                {
                    self.Rigidbody.velocity = Vector3.zero;
                    self.Attack();
                }
                // 否则移动到目标位置
                else
                {
                    self.GameObject.GetComponent<SkeletonAnimation>().AnimationName = "Move";
                    self.GameObject.GetComponent<SkeletonAnimation>().loop = true;
                    self.MoveToTarget();
                }
            }
        }

        [EntitySystem]
        private static void Destroy(this AI_MonsterComponent self)
        {
        }

        // 寻找附近的怪物
        private static void FindTarget(this AI_MonsterComponent self)
        {
            Collider[] colliders = Physics.OverlapSphere(self.GameObject.transform.position, self.DetectionRange);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag(TagHelper.Hero))
                {
                    self.Target = collider.transform;
                    return;
                }
            }

            self.Target = null;
        }

        // 移动到目标
        private static void MoveToTarget(this AI_MonsterComponent self)
        {
            Vector3 direction = (self.Target.position - self.GameObject.transform.position).normalized;
            self.Rigidbody.velocity = direction * self.MoveSpeed;

            // 朝向目标
            // self.GameObject.transform.LookAt(new Vector3(self.target.position.x, self.GameObject.transform.position.y, self.target.position.z));
        }

        // 攻击目标
        private static void Attack(this AI_MonsterComponent self)
        {
            Unit unit = self.GetParent<Unit>();

            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);

            SkillManagerComponent skillManagerComponent = unit.GetComponent<SkillManagerComponent>();
            skillManagerComponent.OnUseSkill(new SkillInfo()
            {
                SkillConfigId = monsterConfig.ActSkillID,
                TargetID = self.Target.GetComponent<UnitId>().Id,
                TargetAngle = self.GameObject.transform.eulerAngles.y,
                TargetPosition = self.Target.transform.position,
            });

            skillManagerComponent.OnUseSkill(new SkillInfo()
            {
                SkillConfigId = monsterConfig.SkillID[0],
                TargetID = self.Target.GetComponent<UnitId>().Id,
                TargetAngle = self.GameObject.transform.eulerAngles.y,
                TargetPosition = self.Target.transform.position,
            });
        }
    }
}