namespace ET.Server
{
    [EntitySystemOf(typeof(BuffS))]
    [FriendOf(typeof(BuffS))]
    public static partial class BuffSSystem
    {
        [EntitySystem]
        private static void Awake(this BuffS self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BuffS self)
        {
        }

        public static void OnInit(this BuffS self, InitBuffData initBuffData, Unit theUnitFrom, Unit theUnitBelongTo, SkillS skill)
        {
            self.InitBuffData = initBuffData;
            self.BuffConfig = BuffConfigCategory.Instance.Get(initBuffData.BuffConfigId);
            self.BuffHandler = BuffDispatcherComponentS.Instance.Get(self.BuffConfig.BuffHandler);
            self.TheUnitFrom = theUnitFrom;
            self.TheUnitBelongTo = theUnitBelongTo;
            self.BuffEndTime = self.BuffConfig.BuffTime;

            self.BuffHandler?.OnInit(self);
        }

        public static void OnUpdate(this BuffS self, float deltaTime)
        {
            self.RunTime += deltaTime;

            if (self.RunTime >= self.BuffEndTime)
            {
                self.BuffState = BuffState.Finished;
                return;
            }

            self.BuffHandler?.OnUpdate(self, deltaTime);
        }

        public static void OnFinished(this BuffS self)
        {
            self.BuffHandler?.OnFinished(self);
        }
    }
}