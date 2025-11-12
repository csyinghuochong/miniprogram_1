using Unity.Mathematics;

namespace ET.Client
{
    public static class EffectHelper
    {
        public static void PlayHitEffect(Unit unit, int skillID)
        {
            //Log.Info("播放受击特效PlayHitEffect:" + skillID);
            //播放受击特效
            if (skillID == 0)
            {
                return;
            }

            SkillConfig skillCof = SkillConfigCategory.Instance.Get(skillID);
            if (skillCof.SkillHitEffectID == 0)
            {
                return;
            }

            if (!EffectConfigCategory.Instance.DataMap.ContainsKey(skillCof.SkillHitEffectID))
            {
                return;
            }

            int angle = 0;

            EffectConfig effectConfig = EffectConfigCategory.Instance.Get(skillCof.SkillHitEffectID);

            InitEffectData playInitEffectBuffData = new InitEffectData();
            playInitEffectBuffData.EffectId = skillCof.SkillHitEffectID;
            playInitEffectBuffData.EffectPosition = float3.zero;
            playInitEffectBuffData.TargetAngle = angle;
            playInitEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playInitEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playInitEffectBuffData);
        }

        public static void PlaySelfEffect(Unit unit, int effectID)
        {
            InitEffectData playInitEffectBuffData = new InitEffectData();
            playInitEffectBuffData.EffectId = effectID;
            playInitEffectBuffData.EffectPosition = float3.zero;
            playInitEffectBuffData.TargetAngle = 0;
            playInitEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playInitEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playInitEffectBuffData);
        }

        public static void PlayEffectPosition(Unit unit, int effectID, float3 position)
        {
            InitEffectData playInitEffectBuffData = new InitEffectData();
            playInitEffectBuffData.EffectId = effectID;
            playInitEffectBuffData.EffectPosition = position;
            playInitEffectBuffData.TargetAngle = 0;
            playInitEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playInitEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playInitEffectBuffData);
        }

        public static void PlayDropEffect(Unit unit, int effectID)
        {
            InitEffectData playInitEffectBuffData = new InitEffectData();
            playInitEffectBuffData.EffectId = effectID;
            playInitEffectBuffData.EffectPosition = float3.zero;
            playInitEffectBuffData.TargetAngle = 0;
            playInitEffectBuffData.EffectTypeEnum = EffectTypeEnum.SkillEffect;
            playInitEffectBuffData.InstanceId = 1;
            unit.GetComponent<EffectViewComponent>()?.EffectFactory(playInitEffectBuffData);
        }
    }
}