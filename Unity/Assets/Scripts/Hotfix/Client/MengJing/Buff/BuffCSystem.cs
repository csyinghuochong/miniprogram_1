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
        }

        public static void OnInit(this BuffC self, InitBuffData initBuffData, Unit theUnitBelongTo)
        {
            self.InitBuffData = initBuffData;
            self.BuffConfig = BuffConfigCategory.Instance.Get(initBuffData.BuffConfigId);
            self.BuffHandler = BuffDispatcherComponentC.Instance.Get(self.BuffConfig.BuffHandler);
            self.TheUnitBelongTo = theUnitBelongTo;
            self.BuffEndTime = initBuffData.BuffEndTime;

            self.BuffHandler?.OnInit(self);
        }

        public static void OnReset(this BuffC self, float endTime)
        {
            self.BuffHandler?.OnReset(self, endTime);
        }

        public static void OnExecute(this BuffC self)
        {
            self.BuffHandler?.OnExecute(self);
        }

        public static void OnUpdate(this BuffC self, float deltaTime)
        {
            self.BuffHandler?.OnUpdate(self, deltaTime);
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

            InitEffectData playInitEffectBuffData = new InitEffectData();
            playInitEffectBuffData.EffectId = self.BuffConfig.BuffEffectID;
            playInitEffectBuffData.TargetAngle = self.InitBuffData.TargetAngle;
            playInitEffectBuffData.EffectTypeEnum = EffectTypeEnum.BuffEffect;
            playInitEffectBuffData.InstanceId = IdGenerater.Instance.GenerateInstanceId();

            //特效类型

            EventSystem.Instance.Publish(self.Root(), new SkillEffect()
            {
                InitEffectData = playInitEffectBuffData,
                Unit = self.TheUnitBelongTo
            });

            self.InitEffectData = playInitEffectBuffData;
            return playInitEffectBuffData.InstanceId;
        }
    }
}