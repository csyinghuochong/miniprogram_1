using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class CameraComponent : Entity, IAwake, ILateUpdate, IDestroy
    {
        public Camera MainCamera;
        private EntityRef<Unit> lookAtUnit;
        public Unit LookAtUnit { get => this.lookAtUnit; set => this.lookAtUnit = value; }
        public Vector3 Offset;
    }
}