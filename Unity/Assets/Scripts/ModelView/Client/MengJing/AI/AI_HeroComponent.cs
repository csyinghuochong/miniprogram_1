using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class AI_HeroComponent : Entity, IAwake, IFixedUpdate, IDestroy
    {
        public float DetectionRange = 20f; // 检测范围
        public float AttackRange; // 攻击范围
        public float MoveSpeed; // 移动速度
        public string EnemyTag = "Monster"; // 怪物标签

        public GameObject GameObject;
        public CharacterController CharacterController;
        public Transform Target;
    }
}