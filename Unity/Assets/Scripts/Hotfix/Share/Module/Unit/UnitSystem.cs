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

        [EntitySystem]
        private static void Destroy(this Unit self)
        {
            self.Scene()?.GetComponent<CrowdComponent>()?.RemoveAgent(self.DtCrowdAgentId);
        }
    }
}