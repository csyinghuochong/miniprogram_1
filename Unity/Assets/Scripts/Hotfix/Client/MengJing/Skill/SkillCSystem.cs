using Unity.Mathematics;

namespace ET.Client
{
    [EntitySystemOf(typeof(SkillC))]
    [FriendOf(typeof(SkillC))]
    public static partial class SkillCSystem
    {
        [EntitySystem]
        private static void Awake(this SkillC self)
        {
        }

        [EntitySystem]
        private static void Destroy(this SkillC self)
        {
        }

        public static void OnInit(this SkillC self, UseSkillInfo useSkillInfo, Unit theUnitFrom)
        {
            self.UseSkillInfo = useSkillInfo;
            self.SkillConfig = SkillConfigCategory.Instance.Get(useSkillInfo.SkillConfigId);
            self.SkillHandler = SkillDispatcherComponentC.Instance.Get(self.SkillConfig.SkillHandler);
            self.SkillState = SkillState.Running;
            self.TheUnitFrom = theUnitFrom;
            if (useSkillInfo.TargetId != 0)
            {
                self.TheUnitTarget = self.Scene().GetComponent<UnitComponent>().Get(useSkillInfo.TargetId);
            }

            self.SkillLiveTime = self.SkillConfig.SkillLiveTime * 1f / 1000;
            self.TargetPosition = useSkillInfo.Position;

            self.SkillHandler?.OnInit(self);
        }

        public static void OnExecute(this SkillC self)
        {
            self.SkillHandler?.OnExecute(self);
        }

        public static void OnUpdate(this SkillC self, float deltaTime)
        {
            self.RunTime += deltaTime;
            if (self.RunTime >= self.SkillConfig.SkillLiveTime)
            {
                self.SkillState = SkillState.Finished;
                return;
            }

            self.SkillHandler?.OnUpdate(self, deltaTime);
        }

        public static void OnFinished(this SkillC self)
        {
            self.SkillHandler?.OnFinished(self);
        }

        public static void PlaySkillEffects(this SkillC self, float3 position, float angle = 0f)
        {
            SkillConfig skillConfig = self.SkillConfig;
            if (skillConfig.SkillHitEffectID == 0)
            {
                return;
            }

            EffectConfig effectConfig = EffectConfigCategory.Instance.Get(skillConfig.SkillEffectID);

            if (string.IsNullOrEmpty(effectConfig.EffectName))
            {
                return;
            }

            EffectData playEffectBuffData = new EffectData();
            playEffectBuffData.TargetID = self.UseSkillInfo.TargetId;
            playEffectBuffData.EffectId = effectConfig.Id; //特效相关配置
            playEffectBuffData.EffectPosition = position; //技能目标点
            playEffectBuffData.EffectAngle = angle;
            playEffectBuffData.TargetAngle = self.UseSkillInfo.Angle; //技能角度
            playEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect; //特效类型
            playEffectBuffData.InstanceId = IdGenerater.Instance.GenerateInstanceId();

            self.EffectInstanceId.Add(playEffectBuffData.InstanceId);

            EventSystem.Instance.Publish(self.Root(), new SkillEffect()
            {
                EffectData = playEffectBuffData,
                Unit = self.TheUnitFrom
            });
        }

        public static void EndSkillEffect(this SkillC self)
        {
            for (int i = 0; i < self.EffectInstanceId.Count; i++)
            {
                EventSystem.Instance.Publish(self.Root(), new SkillEffectFinish
                {
                    EffectInstanceId = self.EffectInstanceId[i],
                    Unit = self.TheUnitFrom
                });
            }
        }
    }
}