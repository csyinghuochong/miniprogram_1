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
            Vector3 lookAt = Vector3.zero;
            if (self.LookAtMode == 0)
            {
                lookAt = UnitHelper.GetMyUnitFromClientScene(self.Root()).GetComponent<GameObjectComponent>().GameObject.transform.position;
            }
            else
            {
                foreach (Unit unit in self.Scene().GetComponent<UnitComponent>().GetAll())
                {
                    if (unit.Type != UnitType.Hero)
                    {
                        continue;
                    }

                    Vector3 unitPos = unit.GetComponent<GameObjectComponent>().GameObject.transform.position;
                    if (lookAt == Vector3.zero)
                    {
                        lookAt = unitPos;
                    }

                    // 看向最上面的英雄
                    if (unit.Position.y > lookAt.y)
                    {
                        lookAt = unitPos;
                    }
                }
            }

            if (lookAt == Vector3.zero)
            {
                return;
            }

            self.MainCamera.transform.position = new Vector3(lookAt.x + self.Offset.x, lookAt.y + self.Offset.y, self.Offset.z);
        }

        [EntitySystem]
        private static void Destroy(this CameraComponent self)
        {
        }
    }
}