using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(TransformSyncComponent))]
    [FriendOf(typeof(TransformSyncComponent))]
    public static partial class TransformSyncComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TransformSyncComponent self)
        {
            self.MyUnit = self.GetParent<Unit>();
            self.GameObjectComponent = self.MyUnit.GetComponent<GameObjectComponent>();

            self.NeedSnap = true;
            self.CurrentPosition = self.MyUnit.Position;
        }

        [EntitySystem]
        private static void Update(this TransformSyncComponent self)
        {
            if (self.NeedSnap)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            var smoothTime = Mathf.Max(0.0001f, self.PositionSmoothTime);
            self.CurrentPosition = Vector3.SmoothDamp(self.CurrentPosition, self.TargetPosition, ref self.PositionVelocity, smoothTime, Mathf.Infinity, Mathf.Max(0.0001f, deltaTime));

            self.GameObjectComponent.UpdatePositon(self.CurrentPosition);
        }

        [EntitySystem]
        private static void Destroy(this TransformSyncComponent self)
        {
        }

        public static void UpdatePositon(this TransformSyncComponent self, float3 target)
        {
            self.TargetPosition = target;
            var distance = Vector3.Distance(self.GameObjectComponent.GameObject.transform.position, self.TargetPosition);
            if (distance >= self.SnapDistance)
            {
                self.NeedSnap = true;
                self.GameObjectComponent.UpdatePositon(self.TargetPosition);
            }
            else
            {
                self.NeedSnap = false;
            }
        }
    }
}