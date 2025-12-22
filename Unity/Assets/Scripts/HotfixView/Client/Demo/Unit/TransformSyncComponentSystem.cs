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
            self.FsmComponent = self.MyUnit.GetComponent<FsmComponent>();

            self.PositionBuffer.EnsureInitialized();
            self.RotationBuffer.EnsureInitialized();
            self.cachedFsmState = FsmStateEnum.FsmIdleState;
        }

        [EntitySystem]
        private static void Update(this TransformSyncComponent self)
        {
            float interpolationTime = Time.unscaledTime - ConfigData.TransformSyncTime / 1000f;
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
            int bufferCount = self.PositionBuffer.Count;

            if (bufferCount == 0) return;

            if (bufferCount >= 2)
            {
                ref PositionState newest = ref self.PositionBuffer.GetRef(0);
                float newerTimestamp = newest.Timestamp;
                Vector3 newerPos = newest.Position;

                for (int i = 0; i < bufferCount - 1; i++)
                {
                    ref PositionState newer = ref self.PositionBuffer.GetRef(i);
                    ref PositionState older = ref self.PositionBuffer.GetRef(i + 1);

                    if (older.Timestamp <= interpolationTime && interpolationTime <= newer.Timestamp)
                    {
                        float t = Mathf.InverseLerp(older.Timestamp, newer.Timestamp, interpolationTime);

                        float sqrDistance = (newer.Position - older.Position).sqrMagnitude;
                        if (sqrDistance < 0.000001f)
                        {
                            return;
                        }

                        if (self.cachedFsmState != FsmStateEnum.FsmRunState)
                        {
                            self.cachedFsmState = FsmStateEnum.FsmRunState;
                            self.FsmComponent.ChangeState(FsmStateEnum.FsmRunState);
                        }

                        self.GameObjectComponent.UpdatePositon(Vector3.Lerp(older.Position, newer.Position, t));
                        self.GameObjectComponent.UpdateScaleX(newer.Position.x - older.Position.x);

                        return;
                    }
                }

                if (self.cachedFsmState != FsmStateEnum.FsmIdleState)
                {
                    self.cachedFsmState = FsmStateEnum.FsmIdleState;
                    self.FsmComponent.ChangeState(FsmStateEnum.FsmIdleState);
                }

                // 防止一开始移动会瞬移第一步，因为服务端只有位置变换才同步位置下来
                if (interpolationTime - newerTimestamp > ConfigData.TransformSyncTime / 1000f)
                {
                    self.ReceiveServerPosition(newerPos);
                }
            }
            else
            {
                ref PositionState target = ref self.PositionBuffer.GetRef(0);
                self.GameObjectComponent.UpdatePositon(Vector3.Lerp(self.GameObjectComponent.GameObject.transform.position, target.Position, Time.unscaledDeltaTime * 5f));
            }
        }

        private static void InterpolateRotation(this TransformSyncComponent self, float interpolationTime)
        {
            int bufferCount = self.RotationBuffer.Count;

            if (bufferCount == 0) return;

            if (bufferCount >= 2)
            {
                for (int i = 0; i < bufferCount - 1; i++)
                {
                    ref RotationState newer = ref self.RotationBuffer.GetRef(i);
                    ref RotationState older = ref self.RotationBuffer.GetRef(i + 1);

                    if (older.Timestamp <= interpolationTime && interpolationTime <= newer.Timestamp)
                    {
                        float t = Mathf.InverseLerp(older.Timestamp, newer.Timestamp, interpolationTime);
                        self.GameObjectComponent.UpdateRotation(Quaternion.Slerp(older.Rotation, newer.Rotation, t));
                        return;
                    }
                }
            }
            else
            {
                ref RotationState target = ref self.RotationBuffer.GetRef(0);
                self.GameObjectComponent.UpdateRotation(Quaternion.Slerp(self.GameObjectComponent.GameObject.transform.rotation, target.Rotation, Time.unscaledDeltaTime * 5f));
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