namespace ET.Server
{
    [FriendOf(typeof(ActivityRechargePointComponent))]
    [EntitySystemOf(typeof(ActivityRechargePointComponent))]
    public static partial class ActivityRechargePointComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ActivityRechargePointComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ActivityRechargePointComponent self)
        {
        }

        [EntitySystem]
        private static void Deserialize(this ActivityRechargePointComponent self)
        {
        }

        public static void Recharge(this ActivityRechargePointComponent self, int recharge)
        {
        }
    }
}