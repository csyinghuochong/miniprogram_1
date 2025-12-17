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
            MapComponent mapComponent = self.Root().GetComponent<MapComponent>();
            Unit myUnit = UnitHelper.GetMyUnitFromClientScene(self.Root());
            if (mapComponent.MapType == MapType.LocalLevel)
            {
                self.LookAtMode = myUnit.GetComponent<NumericComponentC>().GetAsInt(NumericType.BattleMode) == 0 ? 0 : 1;
            }
            else
            {
                self.LookAtMode = 0;
            }

            self.MainCamera = self.Root().GetComponent<GlobalComponent>().MainCamera;

            if (ConfigData.ViewMode == 0)
            {
                self.MainCamera.orthographic = true;
                self.MainCamera.orthographicSize = 25f;
                self.MainCamera.transform.eulerAngles = Vector3.zero;
                self.Offset = new Vector3(0, 0, -100f);
            }
            else
            {
                self.MainCamera.orthographic = false;
                self.MainCamera.transform.eulerAngles = new Vector3(ConfigData.CameraAngle, 0, 0);
                self.Offset = new Vector3(0, -10, -35f);
            }
        }

        [EntitySystem]
        private static void LateUpdate(this CameraComponent self)
        {
            Vector3 targetLookAt = Vector3.zero;
            if (self.LookAtMode == 0)
            {
                targetLookAt = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<GameObjectComponent>().GameObject.transform.position;
                Vector3 targetPosition = new Vector3(targetLookAt.x + self.Offset.x, targetLookAt.y + self.Offset.y, self.Offset.z);
                self.MainCamera.transform.position = targetPosition;
            }
            else
            {
                // 计算所有英雄的中心位置（质心），让摄像机平滑跟随
                Vector3 sumPosition = Vector3.zero;
                int heroCount = 0;

                foreach (Unit unit in self.Scene().GetComponent<UnitComponent>().GetAll())
                {
                    if (unit.Type != UnitType.Hero)
                    {
                        continue;
                    }

                    Vector3 unitPos = unit.GetComponent<GameObjectComponent>().GameObject.transform.position;
                    sumPosition += unitPos;
                    heroCount++;
                }

                if (heroCount > 0)
                {
                    targetLookAt = sumPosition / heroCount;
                }

                if (targetLookAt == Vector3.zero)
                {
                    return;
                }

                Vector3 targetPosition = new Vector3(targetLookAt.x + self.Offset.x, targetLookAt.y + self.Offset.y, self.Offset.z);
                float distance = Vector3.Distance(self.MainCamera.transform.position, targetPosition);

                if (distance > 10)
                {
                    // 距离过大，直接跳转
                    self.MainCamera.transform.position = targetPosition;
                }
                else
                {
                    // 距离较近，平滑过渡
                    float smoothSpeed = 5f; // 平滑速度，可调整
                    self.MainCamera.transform.position = Vector3.Lerp(self.MainCamera.transform.position, targetPosition, smoothSpeed * Time.deltaTime);
                }
            }
        }

        [EntitySystem]
        private static void Destroy(this CameraComponent self)
        {
        }
    }
}