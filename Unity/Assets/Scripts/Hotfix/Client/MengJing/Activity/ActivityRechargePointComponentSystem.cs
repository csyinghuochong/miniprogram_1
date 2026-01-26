namespace ET.Client
{
    [EntitySystemOf(typeof(ActivityRechargePointComponentC))]
    public static partial class ActivityRechargePointComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ActivityRechargePointComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ActivityRechargePointComponentC self)
        {
        }

        public static void Clear(this ActivityRechargePointComponentC self)
        {
            self.RechargePoint = 0;
            self.ReceivedRechargePointRewardIds.Clear();
        }
    }
}