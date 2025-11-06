using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(CameraComponent))]
    [FriendOf(typeof(CameraComponent))]
    public static partial class CameraComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CameraComponent self)
        {
            self.MainCamera = Camera.main;
            self.LookAtUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());

            self.MainCamera.orthographicSize = 20f;
        }

        [EntitySystem]
        private static void LateUpdate(this CameraComponent self)
        {
            self.MainCamera.transform.position = new Vector3(self.LookAtUnit.Position.x, self.LookAtUnit.Position.y, -10f);
        }

        [EntitySystem]
        private static void Destroy(this CameraComponent self)
        {
        }
    }
}