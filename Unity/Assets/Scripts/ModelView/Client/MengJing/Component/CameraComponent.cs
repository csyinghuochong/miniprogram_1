using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class CameraComponent : Entity, IAwake, ILateUpdate, IDestroy
    {
        public Camera MainCamera;

        public Vector3 Offset;

        public int LookAtMode = 0; // 0: 玩家 1: 英雄
    }
}