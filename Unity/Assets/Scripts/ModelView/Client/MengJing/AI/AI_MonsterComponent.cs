using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class AI_MonsterComponent : Entity, IAwake, IUpdate, IDestroy
    {
        public float DetectionRange = 20f; // 检测范围
        public float AttackRange; // 攻击范围
        public float MoveSpeed; // 移动速度

        public GameObject GameObject;
        public Rigidbody Rigidbody;
        public Transform Target;
    }
}