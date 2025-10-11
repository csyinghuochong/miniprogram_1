using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(Skill))]
    [FriendOf(typeof(Skill))]
    public static partial class SkillSystem
    {
        [EntitySystem]
        private static void Awake(this Skill self)
        {
        }

        [EntitySystem]
        private static void Destroy(this Skill self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.EffectPath, self.EffectGameObject);
        }

        public static void BaseOnInit(this Skill self, SkillInfo skillInfo, Unit theUnitFrom)
        {
            self.SkillInfo = skillInfo;
            self.SkillConfig = SkillConfigCategory.Instance.Get(skillInfo.SkillConfigId);
            self.TheUnitFrom = theUnitFrom;
            if (skillInfo.TargetID != 0)
            {
                self.TheUnitTarget = self.Scene().GetComponent<UnitComponent>().Get(skillInfo.TargetID);
            }
            self.SkillState = SkillState.Running;
            self.SkillLiveTime = self.SkillConfig.SkillLiveTime * 1f / 1000;
            self.TargetPosition = skillInfo.TargetPosition;
            self.NowPosition = self.TargetPosition;
        }

        public static void BaseOnUpdate(this Skill self)
        {
            if (self.SkillLiveTime <= 0)
            {
                self.SkillState = SkillState.Finished;
            }
        }

        public static void InitSelfBuff(this Skill self)
        {
        }

        public static void PlaySkillEffects(this Skill self)
        {
            SkillConfig skillConfig = self.SkillConfig;
            if (skillConfig.SkillHitEffectID == 0)
            {
                return;
            }

            EffectConfig effectConfig = EffectConfigCategory.Instance.Get(skillConfig.SkillHitEffectID);

            if (string.IsNullOrEmpty(effectConfig.EffectName))
            {
                return;
            }

            self.EffectPath = $"Assets/Bundles/Effect/SkillEffect/{effectConfig.EffectName}.prefab";

            self.Root().GetComponent<GameObjectLoadComponent>().AddLoadQueue(self.EffectPath, self.InstanceId, true, self.OnLoadGameObject);
        }

        public static void EndSkillEffect(this Skill self)
        {
            self.Root().GetComponent<GameObjectLoadComponent>().RecoverGameObject(self.EffectPath, self.EffectGameObject);
            self.EffectPath = null;
            self.EffectGameObject = null;
        }

        private static void OnLoadGameObject(this Skill self, GameObject gameObject, long instanceId)
        {
            if (instanceId != self.InstanceId)
            {
                if (gameObject != null)
                {
                    UnityEngine.Object.DestroyImmediate(gameObject);
                }

                return;
            }

            self.EffectGameObject = gameObject;
            self.SkillHandler.OnEffectLoaded(self);
        }
    }
}