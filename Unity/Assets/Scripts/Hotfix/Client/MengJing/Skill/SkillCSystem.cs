using Cysharp.Text;
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

        public static void OnInit(this SkillC self, InitSkillData initSkillData, Unit theUnitFrom)
        {
            self.InitSkillData = initSkillData;
            self.SkillConfig = SkillConfigCategory.Instance.Get(initSkillData.SkillConfigId);
            self.SkillHandler = SkillDispatcherComponentC.Instance.Get(self.SkillConfig.SkillHandler);
            self.SkillState = SkillState.Running;
            self.TheUnitFrom = theUnitFrom;
            if (initSkillData.TargetId != 0)
            {
                self.TheUnitTarget = self.Scene().GetComponent<UnitComponent>().Get(initSkillData.TargetId);
            }

            self.TargetPosition = initSkillData.TargetPosition;

            if (!string.IsNullOrEmpty(self.SkillConfig.SkillMusic) && self.SkillConfig.SkillMusic != "0")
            {
                EventSystem.Instance.Publish(self.Root(), new SkillSound() { Asset = ZString.Format("SkillAudio/{0}", self.SkillConfig.SkillMusic) });
            }

            self.SkillHandler?.OnInit(self);
        }

        public static void OnExecute(this SkillC self)
        {
            self.SkillHandler?.OnExecute(self);
        }

        public static void OnUpdate(this SkillC self, float deltaTime)
        {
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

            InitEffectData playInitEffectBuffData = new InitEffectData();
            playInitEffectBuffData.TargetId = self.InitSkillData.TargetId;
            playInitEffectBuffData.EffectId = effectConfig.Id; //特效相关配置
            playInitEffectBuffData.EffectPosition = position; //技能目标点
            playInitEffectBuffData.EffectAngle = angle;
            playInitEffectBuffData.TargetAngle = self.InitSkillData.Angle; //技能角度
            playInitEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect; //特效类型
            playInitEffectBuffData.InstanceId = IdGenerater.Instance.GenerateInstanceId();

            self.EffectInstanceId.Add(playInitEffectBuffData.InstanceId);

            EventSystem.Instance.Publish(self.Root(), new SkillEffect()
            {
                InitEffectData = playInitEffectBuffData,
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