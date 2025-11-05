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

        public static void OnInit(this BuffS self, BuffData buffData, Unit theUnitFrom, Unit theUnitBelongTo, SkillS skill)
        {
            self.BuffData = buffData;
            self.BuffConfig = BuffConfigCategory.Instance.Get(buffData.BuffConfigId);
            self.BuffHandler = BuffDispatcherComponentS.Instance.Get(self.BuffConfig.BuffHandler);
            self.TheUnitFrom = theUnitFrom;
            self.TheUnitBelongTo = theUnitBelongTo;
            self.BuffEndTime = self.BuffConfig.BuffTime;

            self.BuffHandler?.OnInit(self);
        }

        public static void OnUpdate(this BuffS self, float deltaTime)
        {
            self.BuffEndTime -= deltaTime;
            if (self.BuffEndTime <= 0)
            {
                self.BuffState = BuffState.Finished;
            }

            self.BuffHandler?.OnUpdate(self, deltaTime);
        }

        public static void OnFinished(this BuffS self)
        {
            self.BuffHandler?.OnFinished(self);
        }
    }
}