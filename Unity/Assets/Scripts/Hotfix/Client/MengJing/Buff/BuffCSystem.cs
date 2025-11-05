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

        public static void OnInit(this BuffC self, BuffData buffData, Unit theUnitBelongTo)
        {
            self.BuffData = buffData;
            self.BuffConfig = BuffConfigCategory.Instance.Get(buffData.BuffConfigId);
            self.BuffHandler = BuffDispatcherComponentC.Instance.Get(self.BuffConfig.BuffHandler);
            self.TheUnitBelongTo = theUnitBelongTo;
            self.BuffEndTime = buffData.BuffEndTime;

            self.BuffHandler?.OnInit(self);
        }

        public static void OnReset(this BuffC self, float endTime)
        {
            self.BuffHandler?.OnReset(self, endTime);
        }

        public static void OnUpdate(this BuffC self, float deltaTime)
        {
            self.RunTime += deltaTime;

            self.BuffHandler?.OnUpdate(self);
        }

        public static void OnFinished(this BuffC self)
        {
            self.BuffHandler?.OnFinished(self);
        }

        public static long PlayBuffEffects(this BuffC self)
        {
            if (self.BuffConfig.BuffEffectID == 0)
            {
                return 0;
            }

            EffectData playEffectBuffData = new EffectData();
            playEffectBuffData.EffectId = self.BuffConfig.BuffEffectID;
            playEffectBuffData.TargetAngle = self.BuffData.TargetAngle;
            playEffectBuffData.EffectTypeEnum = EffectTypeEnum.BuffEffect;
            playEffectBuffData.InstanceId = IdGenerater.Instance.GenerateInstanceId();

            //特效类型

            EventSystem.Instance.Publish(self.Root(), new SkillEffect()
            {
                EffectData = playEffectBuffData,
                Unit = self.TheUnitBelongTo
            });

            self.EffectData = playEffectBuffData;
            return playEffectBuffData.InstanceId;
        }
    }
}