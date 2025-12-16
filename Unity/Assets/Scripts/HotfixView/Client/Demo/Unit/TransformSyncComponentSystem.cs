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
        }

        [EntitySystem]
        private static void Update(this TransformSyncComponent self)
        {
            float interpolationTime = self.MyUnit.MainHero ? Time.unscaledTime : Time.unscaledTime - ConfigData.TransformSyncTime / 1000f;
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
                float newerTimestamp = float.MinValue;
                Vector3 newerPos = Vector3.zero;
                for (int i = 0; i < self.PositionBuffer.Count - 1; i++)
                {
                    var newer = self.PositionBuffer[i];
                    var older = self.PositionBuffer[i + 1];

                    if (newer.Timestamp > newerTimestamp)
                    {
                        newerTimestamp = newer.Timestamp;
                        newerPos = newer.Position;
                    }

                    if (older.Timestamp <= interpolationTime && interpolationTime <= newer.Timestamp)
                    {
                        float t = Mathf.InverseLerp(older.Timestamp, newer.Timestamp, interpolationTime);

                        if (newer.Position == older.Position)
                        {
                            return;
                        }

                        EventSystem.Instance.Publish(self.Scene(), new MoveStart() { Unit = self.MyUnit });

                        self.GameObjectComponent.UpdatePositon(Vector3.Lerp(older.Position, newer.Position, t));
                        self.GameObjectComponent.UpdateScaleX(newer.Position.x - older.Position.x);

                        return;
                    }
                }
                
                EventSystem.Instance.Publish(self.Scene(), new MoveStop() { Unit = self.MyUnit });

                // 防止一开始移动会瞬移第一步，因为服务端只有位置变换才同步位置下来
                if (interpolationTime - newerTimestamp > 0.1f)
                {
                    self.ReceiveServerPosition(newerPos);
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