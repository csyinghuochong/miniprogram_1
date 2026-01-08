namespace ET.Server
{
    [EntitySystemOf(typeof(Buff))]
    [FriendOf(typeof(Buff))]
    public static partial class BuffSystem
    {
        [EntitySystem]
        private static void Awake(this Buff self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Buff self)
        {
        }

        public static void OnInit(this Buff self, InitBuffData initBuffData, Unit theUnitFrom, Unit theUnitBelongTo, Skill skill)
        {
            self.InitBuffData = initBuffData;
            self.BuffConfig = BuffConfigCategory.Instance.Get(initBuffData.BuffConfigId);
            self.BuffHandler = BuffDispatcherComponent.Instance.Get(self.BuffConfig.BuffHandler);
            self.TheUnitFrom = theUnitFrom;
            self.TheUnitBelongTo = theUnitBelongTo;
            self.BuffEndTime = self.BuffConfig.BuffTime;

            self.BuffHandler?.OnInit(self);
        }

        public static void OnUpdate(this Buff self, float deltaTime)
        {
            self.BuffHandler?.OnUpdate(self, deltaTime);
        }

        public static void OnFinished(this Buff self)
        {
            self.BuffHandler?.OnFinished(self);
        }
    }
}