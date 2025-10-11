using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(AI_HeroComponent))]
    [FriendOf(typeof(AI_HeroComponent))]
    public static partial class AI_HeroComponentSystem
    {
        [EntitySystem]
        private static void Awake(this AI_HeroComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            GameObjectComponent gameObjectComponent = unit.GetComponent<GameObjectComponent>();
            self.GameObject = gameObjectComponent.GameObject;
            self.CharacterController = gameObjectComponent.GameObject.GetComponent<CharacterController>();

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);
            self.AttackRange = heroConfig.AtkDistance;
            self.MoveSpeed = (float)heroConfig.MoveSpeed;
        }

        [EntitySystem]
        private static void FixedUpdate(this AI_HeroComponent self)
        {
            if (self.Target == null)
            {
                self.FindTarget();
            }

            if (self.Target != null)
            {
                float distance = Vector3.Distance(self.GameObject.transform.position, self.Target.position);

                // 如果在攻击范围内，进行攻击
                if (distance <= self.AttackRange)
                {
                    self.Attack();
                }
                // 否则移动到目标位置
                else
                {
                    self.MoveToTarget();
                }
            }
        }

        [EntitySystem]
        private static void Destroy(this AI_HeroComponent self)
        {
        }

        // 寻找附近的怪物
        private static void FindTarget(this AI_HeroComponent self)
        {
            Collider[] colliders = Physics.OverlapSphere(self.GameObject.transform.position, self.DetectionRange);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag(self.EnemyTag))
                {
                    self.Target = collider.transform;
                    return;
                }
            }

            self.Target = null;
        }

        // 移动到目标
        private static void MoveToTarget(this AI_HeroComponent self)
        {
            Vector3 direction = (self.Target.position - self.GameObject.transform.position).normalized;
            direction.y = 0; // 忽略Y轴，保持在同一平面
            self.CharacterController.Move(direction * self.MoveSpeed * Time.deltaTime);

            // 朝向目标
            // self.GameObject.transform.LookAt(new Vector3(self.target.position.x, self.GameObject.transform.position.y, self.target.position.z));
        }

        // 攻击目标
        private static void Attack(this AI_HeroComponent self)
        {
            // Debug.Log("攻击怪物！");
            Unit unit = self.GetParent<Unit>();

            HeroConfig heroConfig = HeroConfigCategory.Instance.Get(unit.ConfigId);

            SkillManagerComponent skillManagerComponent = unit.GetComponent<SkillManagerComponent>();
            skillManagerComponent.OnUseSkill(new SkillInfo()
            {
                SkillConfigId = heroConfig.AtkID,
                TargetID = self.Target.GetComponent<UnitId>().Id,
                TargetAngle = self.GameObject.transform.eulerAngles.y,
                TargetPosition = self.GameObject.transform.position,
            });
        }
    }
}