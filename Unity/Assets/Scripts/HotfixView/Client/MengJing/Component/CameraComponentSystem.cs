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
            self.MainCamera = self.Root().GetComponent<GlobalComponent>().MainCamera;
            self.LookAtUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());
            
            if (self.Root().GetComponent<GlobalComponent>().ViewMode == 0)
            {
                self.MainCamera.orthographic = true;
                self.MainCamera.orthographicSize = 25f;
                self.MainCamera.transform.eulerAngles = Vector3.zero;
                self.Offset = new Vector3(0, 0, -100f);
            }
            else
            {
                self.MainCamera.orthographic = false;
                self.MainCamera.transform.eulerAngles = new Vector3(-30f, 0, 0);
                self.Offset = new Vector3(0, -25, -35f);
            }
        }

        [EntitySystem]
        private static void LateUpdate(this CameraComponent self)
        {
            self.MainCamera.transform.position = new Vector3(self.LookAtUnit.Position.x + self.Offset.x, self.LookAtUnit.Position.y + self.Offset.y, self.Offset.z);
        }

        [EntitySystem]
        private static void Destroy(this CameraComponent self)
        {
        }
    }
}