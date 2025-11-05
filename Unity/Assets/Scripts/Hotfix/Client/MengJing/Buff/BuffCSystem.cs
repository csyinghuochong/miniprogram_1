namespace ET.Client
{
    [EntitySystemOf(typeof(BuffC))]
    [FriendOf(typeof(BuffC))]
    public static partial class BuffCSystem
    {
        [EntitySystem]
        private static void Awake(this BuffC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this BuffC self)
        {
            self.OnFinished();
        }

        public static void OnInit(this BuffC self, BuffData buffData, Unit theUnitFrom, Unit theUnitBelongto, SkillC skillC)
        {
            // self.PassTime = 0;
            // self.IsTrigger = false;
            // self.BuffData = buffData;
            // self.TheUnitFrom = theUnitFrom;
            // self.TheUnitBelongto = theUnitBelongto;
            // self.BuffState = BuffState.Running;
            // self.BeginTime = TimeHelper.ServerNow();
            // self.mSkillConf = SkillConfigCategory.Instance.Get(buffData.SkillId);
            // self.mBuffConfig = SkillBuffConfigCategory.Instance.Get(buffData.BuffId);
            // self.DelayTime = self.mBuffConfig.BuffDelayTime;
            // self.BuffEndTime = CheckBuffTime(theUnitBelongto, self.mBuffConfig) +
            //         1000 * (int)self.GetTianfuProAdd((int)BuffAttributeEnum.AddBuffTime) + TimeHelper.ServerNow();
            // self.BuffEndTime = buffData.BuffEndTime > 0? buffData.BuffEndTime : self.BuffEndTime;
            // self.InterValTime = self.mBuffConfig.BuffLoopTime * 1000;
            // self.InterValTimeBegin = TimeHelper.ServerNow();
            // self.NowBuffValue = 0f;
        }

        public static void OnUpdate(this BuffC self)
        {
        }

        public static void OnFinished(this BuffC self)
        {
        }
    }
}