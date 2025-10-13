namespace ET.Client
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
        
        public static void BaseOnInit(this Buff self, BuffData buffData, Unit theUnitFrom, Unit theUnitBelongto)
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
    }
}