namespace ET.Client
{
    [EntitySystemOf(typeof(ActivityServerOpenComponentC))]
    public static partial class ActivityServerOpenComponentCSystem
    {
        [EntitySystem]
        private static void Awake(this ActivityServerOpenComponentC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this ActivityServerOpenComponentC self)
        {
        }

        public static void Clear(this ActivityServerOpenComponentC self)
        {
            self.ReceivedServerOpenRewardIds.Clear();
        }
    }
}