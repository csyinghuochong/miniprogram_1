using Unity.Mathematics;

namespace ET
{
    [EntitySystemOf(typeof(Unit))]
    [FriendOf(typeof(Unit))]
    public static partial class UnitSystem
    {
        [EntitySystem]
        private static void Awake(this Unit self, int configId)
        {
            self.ConfigId = configId;
        }

        public static int Type(this Unit self)
        {
            return self.Type;
        }

        public static void SetPosition(this Unit self, float3 value)
        {
            float3 oldPos = self.Position;
            self.Position = value;

            EventSystem.Instance.Publish(self.Scene(), new ChangePosition() { Unit = self, OldPos = oldPos });
        }

        public static void SetRotation(this Unit self, quaternion value)
        {
            self.Rotation = value;
            EventSystem.Instance.Publish(self.Scene(), new ChangeRotation() { Unit = self });
        }
    }
}