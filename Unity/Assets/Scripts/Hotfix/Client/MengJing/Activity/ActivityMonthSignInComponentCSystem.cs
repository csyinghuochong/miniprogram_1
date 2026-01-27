namespace ET.Client
{
    [EntitySystemOf(typeof(ActivityMonthSignInComponentC))]
    public static partial class ActivityMonthSignInComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this ActivityMonthSignInComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ActivityMonthSignInComponentC self)
        {
        }

        public static void Clear(this ActivityMonthSignInComponentC self)
        {
            self.LastSignInTime = 0;
            self.TotalSignInDay = 0;
            self.ReceivedMonthSignInIds.Clear();
        }
    }
}