using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class CameraComponent : Entity, IAwake, ILateUpdate, IDestroy
    {
        public Camera MainCamera;
        public Vector3 Offset;
    }
}