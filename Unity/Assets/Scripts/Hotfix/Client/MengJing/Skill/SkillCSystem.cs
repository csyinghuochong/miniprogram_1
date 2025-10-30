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
            self.OnFinished();
        }

        public static void OnInit(this SkillC self, SkillInfo skillInfo, Unit theUnitFrom)
        {
            self.SkillInfo = skillInfo;
            self.SkillConfig = SkillConfigCategory.Instance.Get(skillInfo.SkillConfigId);
            self.SkillHandler = SkillDispatcherComponentC.Instance.Get(self.SkillConfig.SkillHandler);
            self.SkillState = SkillState.Running;
            self.TheUnitFrom = theUnitFrom;
            if (skillInfo.TargetID != 0)
            {
                self.TheUnitTarget = self.Scene().GetComponent<UnitComponent>().Get(skillInfo.TargetID);
            }

            self.SkillLiveTime = self.SkillConfig.SkillLiveTime * 1f / 1000;
            self.TargetPosition = skillInfo.TargetPosition;
            self.NowPosition = self.TargetPosition;

            self.SkillHandler.OnInit(self);
        }

        public static void OnExecute(this SkillC self)
        {
            self.SkillHandler.OnExecute(self);
        }

        public static void OnUpdate(this SkillC self, float deltaTime)
        {
            self.SkillHandler.OnUpdate(self, deltaTime);
        }

        public static void OnFinished(this SkillC self)
        {
            self.SkillHandler.OnFinished(self);
        }

        public static void InitSelfBuff(this SkillC self)
        {
        }

        // public static void PlaySkillEffects(this SkillC self)
        // {
        //     SkillConfig skillConfig = self.SkillConfig;
        //     if (skillConfig.SkillHitEffectID == 0)
        //     {
        //         return;
        //     }
        //
        //     EffectConfig effectConfig = EffectConfigCategory.Instance.Get(skillConfig.SkillHitEffectID);
        //
        //     if (string.IsNullOrEmpty(effectConfig.EffectName))
        //     {
        //         return;
        //     }
        //
        //     self.EffectPath = ZString.Format("Assets/Bundles/Effect/SkillEffect/{0}.prefab", effectConfig.EffectName);
        //
        //     self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.EffectPath, self.InstanceId, true, self.OnLoadGameObject);
        // }
        //
        // public static void EndSkillEffect(this SkillC self)
        // {
        //     self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.EffectPath, self.EffectGameObject);
        //     self.EffectPath = null;
        //     self.EffectGameObject = null;
        // }
        //
        // private static void OnLoadGameObject(this SkillC self, GameObject gameObject, long instanceId)
        // {
        //     if (instanceId != self.InstanceId)
        //     {
        //         if (gameObject != null)
        //         {
        //             UnityEngine.Object.DestroyImmediate(gameObject);
        //         }
        //
        //         return;
        //     }
        //
        //     self.EffectGameObject = gameObject;
        //     self.SkillHandlerC.OnEffectLoaded(self);
        // }
    }
}