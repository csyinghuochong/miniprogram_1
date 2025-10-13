using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class SetUnitTransformComponent : Entity, IAwake, IUpdate
    {
        public Transform Transform;
        private EntityRef<Unit> unit;
        public Unit Unit { get => unit; set => unit = value; }
    }
}