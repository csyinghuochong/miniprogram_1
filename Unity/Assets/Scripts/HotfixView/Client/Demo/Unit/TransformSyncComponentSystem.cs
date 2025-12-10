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
            self.GameObjectComponent = self.GetParent<Unit>().GetComponent<GameObjectComponent>();
        }

        [EntitySystem]
        private static void Update(this TransformSyncComponent self)
        {
            float interpolationTime = Time.unscaledTime - self.InterpolationBackTime;
            self.InterpolatePosition(interpolationTime);
            // self.InterpolateRotation(interpolationTime);
        }

        [EntitySystem]
        private static void Destroy(this TransformSyncComponent self)
        {
            self.PositionBuffer.Clear();
            self.RotationBuffer.Clear();
        }

        private static void InterpolatePosition(this TransformSyncComponent self, float interpolationTime)
        {
            if (self.PositionBuffer.Count >= 2)
            {
                for (int i = 0; i < self.PositionBuffer.Count - 1; i++)
                {
                    var newer = self.PositionBuffer[i];
                    var older = self.PositionBuffer[i + 1];

                    if (older.Timestamp <= interpolationTime && interpolationTime <= newer.Timestamp)
                    {
                        float t = Mathf.InverseLerp(older.Timestamp, newer.Timestamp, interpolationTime);
                        
                        self.GameObjectComponent.UpdatePositon(Vector3.Lerp(older.Position, newer.Position, t));
                        self.GameObjectComponent.UpdateScaleX((newer.Position - older.Position).x);
                        return;
                    }
                }
            }
            else if (self.PositionBuffer.Count == 1)
            {
                var target = self.PositionBuffer[0].Position;
                self.GameObjectComponent.UpdatePositon(Vector3.Lerp(self.GameObjectComponent.GameObject.transform.position, target, Time.unscaledDeltaTime * 5f));
            }
        }

        private static void InterpolateRotation(this TransformSyncComponent self, float interpolationTime)
        {
            if (self.RotationBuffer.Count >= 2)
            {
                for (int i = 0; i < self.RotationBuffer.Count - 1; i++)
                {
                    var newer = self.RotationBuffer[i];
                    var older = self.RotationBuffer[i + 1];

                    if (older.Timestamp <= interpolationTime && interpolationTime <= newer.Timestamp)
                    {
                        float t = Mathf.InverseLerp(older.Timestamp, newer.Timestamp, interpolationTime);
                        self.GameObjectComponent.UpdateRotation(Quaternion.Slerp(older.Rotation, newer.Rotation, t));
                        return;
                    }
                }
            }
            else if (self.RotationBuffer.Count == 1)
            {
                var target = self.RotationBuffer[0].Rotation;
                self.GameObjectComponent.UpdateRotation(Quaternion.Slerp(self.GameObjectComponent.GameObject.transform.rotation, target, Time.unscaledDeltaTime * 5f));
            }
        }

        public static void ReceiveServerPosition(this TransformSyncComponent self, Vector3 pos)
        {
            PositionState state = new PositionState
            {
                Timestamp = Time.unscaledTime,
                Position = pos
            };
            self.PositionBuffer.Add(state);
        }

        public static void ReceiveServerRotation(this TransformSyncComponent self, Quaternion rot)
        {
            RotationState state = new RotationState
            {
                Timestamp = Time.unscaledTime,
                Rotation = rot
            };
            self.RotationBuffer.Add(state);
        }
    }
}